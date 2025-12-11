/*
The entry point to our program.
*/
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
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/app-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .WriteTo.File(
        "logs/errors-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        restrictedToMinimumLevel: LogEventLevel.Error
    )
    .CreateLogger();

// Use Serilog as the logging provider
builder.Host.UseSerilog();

// Documentation
builder.Services.AddOpenApi();

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
    app.MapOpenApi();
    app.MapScalarApiReference();
    // Use this line because openAPI does not generate swagger by default.
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Music API");
    });
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
