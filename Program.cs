/*
The entry point to our program.
*/

using MusicDatabaseApi.Endpoints;
using MusicDatabaseApi.Models;
using MusicDatabaseApi.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IMusicRepository, InMemoryMusicRepository>();

var app = builder.Build();

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

app.UseHttpsRedirection();

// Map all album endpoints
app.MapAlbumEndpoints();

app.Run();
