/*
The entry point to our program.
*/
using Microsoft.EntityFrameworkCore;
using MusicDatabaseApi.Data;
using MusicDatabaseApi.Endpoints;
using MusicDatabaseApi.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

/// Be very careful, we have two different IMUsicRepository
/// We must specify which one to use.
builder.Services.AddScoped<IMusicRepository, SqlMusicRepository>();
builder.Services.AddSingleton<AlbumParameters, AlbumParameters>();

// You need a valid connection string in the app setting for it to create a .db file.
builder.Services.AddDbContext<MusicDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

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

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MusicDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
