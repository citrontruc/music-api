/*
The entry point to our program.
*/
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.EntityFrameworkCore;
using MusicDatabaseApi.Data;
using MusicDatabaseApi.Endpoints;
using MusicDatabaseApi.Repositories;
//using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region General services

// Configure logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

// Use Serilog as the logging provider
builder.Host.UseSerilog();

// Api versioning.
// Do this before documentation or multiple versions might be ignored.
builder
    .Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(3);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        // note: this option is only necessary when versioning by url segment.
        // This is for doc: replaces version number in url for cleaner doc
        options.SubstituteApiVersionInUrl = true;
    });

// Security
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddProblemDetails();

// Documentation
// If you don't have these lines, no operations are marked in swagger.
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<ConfigureOpenApiOptions>();
});

// Necessary to generate your swagger file.
builder.Services.AddSwaggerGen();

// Options and configuration of swagger are handled by our ConfigureSwaggerOptions
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

#endregion

#region Declaration of services

/// Be very careful, if you have multiple IMUsicRepository
/// We must specify which one to use.
builder.Services.AddScoped<IAlbumRepository, SqlAlbumRepository>();
builder.Services.AddScoped<IArtistRepository, SqlArtistRepository>();
builder.Services.AddSingleton<PaginationParameters, PaginationParameters>();

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

// Security
app.UseHttpsRedirection();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MusicDbContext>();
    await context.Database.MigrateAsync().ConfigureAwait(false);
}

# endregion

#region Declare endpoints

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasDeprecatedApiVersion(new ApiVersion(1)) // Can't input v0
    .HasApiVersion(new ApiVersion(2))
    .HasApiVersion(new ApiVersion(3))
    .ReportApiVersions()
    .Build();

// We need to be authenticated in order to interact with the api.
RouteGroupBuilder versionGroup = app.MapGroup("api/v{version:apiVersion}/")
    .WithApiVersionSet(apiVersionSet)
    //.HasApiVersion(1) // Map to V1
    //.HasApiVersion(2) // Map to V2
    .RequireAuthorization();

// Map all album endpoints
versionGroup.MapAlbumEndpoints();
versionGroup.MapArtistEndpoints();

#endregion

#region Development

if (app.Environment.IsDevelopment())
{
    /*
    var descriptions = app.DescribeApiVersions();
    foreach (var description in descriptions)
    {
        app.MapOpenApi($"/openapi/{description.GroupName}/openapi.json")
            .WithGroupName(description.GroupName);
    }
    */

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });

    /*
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Music Database API")
            .WithTheme(ScalarTheme.Default)
            .WithOpenApiRoutePattern("/openapi/{documentName}/openapi.json");
    });
    */
}

#endregion

#region Run application

try
{
    Log.Information("Starting web application");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}

#endregion
