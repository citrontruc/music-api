/*
The entry point to our program.
*/
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using MusicDatabaseApi.Data;
using MusicDatabaseApi.Endpoints;
using MusicDatabaseApi.Repositories;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

#region General services

// Logs are written to files (simple file for base logs and special file for errors) and to console.
/*
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/app-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        retainedFileCountLimit: 7
    )
    .WriteTo.File(
        "logs/errors-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        restrictedToMinimumLevel: LogEventLevel.Error,
        retainedFileCountLimit: 7
    )
    .CreateLogger();
*/

// Instead of putting configuration in code, put it in appsettings.
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

// Use Serilog as the logging provider
builder.Host.UseSerilog();

// Documentation
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<OpenApiVersionTransformer>();
});

// Api versioning
builder
    .Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(2);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        // This is pureluy for doc: replaces version number in url for cleaner doc
        options.SubstituteApiVersionInUrl = true;
    });

// Security
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddProblemDetails();

#endregion

#region Declaration of services

/// Be very careful, if you have multiple IMUsicRepository
/// We must specify which one to use.
builder.Services.AddScoped<IMusicRepository, SqlMusicRepository>();
builder.Services.AddSingleton<AlbumParameters, AlbumParameters>();

// You need a valid connection string in the app setting for it to create a .db file.
builder.Services.AddDbContext<MusicDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

#endregion

#region Build components

var app = builder.Build();

// Exception handler should be FIRST because after logging, we fail.
app.UseMiddleware<ExceptionLoggingMiddleware>();

// Then request logging
app.UseMiddleware<RequestLoggingMiddleware>();

// Authentication
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    // Map OpenAPI specs for each version. This is the building block.
    // Scalar and swagger use openAPI as base.
    foreach (var description in provider.ApiVersionDescriptions)
    {
        app.MapOpenApi($"/openapi/{description.GroupName}.json")
            .WithGroupName(description.GroupName);

        // Configure Scalar to accept multiple versions
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Music Database API")
                .WithTheme(ScalarTheme.Default)
                .WithOpenApiRoutePattern($"/openapi/{description.GroupName}.json");
            ;
        });

        // You don't need both swagger and scalar but here is both if you need it.
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint(
                $"/openapi/{description.GroupName}.json",
                description.GroupName.ToUpperInvariant()
            );
        });
    }
}

// Security
app.UseHttpsRedirection();

// Map all album endpoints
app.MapAlbumEndpoints();

// Create database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MusicDbContext>();
    context.Database.EnsureCreated();
}

#endregion

#region Run application

try
{
    Log.Information("Starting web application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

#endregion
