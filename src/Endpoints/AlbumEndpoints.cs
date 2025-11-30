/*
A class to add the endpoints to add albums to our database.
*/

namespace MusicDatabaseApi.Endpoints
{
    using Microsoft.AspNetCore.Mvc;
    using MusicDatabaseApi.Models;
    using MusicDatabaseApi.Repositories;

    public static class AlbumEndpoints
    {
        /// <summary>
        /// Extension of the app in order to add endpoints to retrieve albums
        /// </summary>
        /// <param name="app"></param>
        public static void MapAlbumEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/albums").WithTags("Albums");

            group
                .MapPost("/", CreateAlbum)
                .WithName("CreateAlbum")
                .WithSummary("Create a new album");

            group
                .MapGet("/", GetAlbums)
                .WithName("GetAlbums")
                .WithSummary("Get all albums or search by name/artist");

            group
                .MapGet("/{id:guid}", GetAlbumById)
                .WithName("GetAlbumById")
                .WithSummary("Get a specific album by ID");
        }

        private static IResult CreateAlbum(CreateAlbumRequest request, IMusicRepository repo)
        {
            if (
                string.IsNullOrWhiteSpace(request.Name)
                || string.IsNullOrWhiteSpace(request.ArtistName)
            )
            {
                return Results.BadRequest("Album name and artist name are required.");
            }

            var album = repo.CreateAlbum(request);
            return Results.Created($"/api/albums/{album.Id}", album);
        }

        private static IResult GetAlbums(
            string? name,
            string? artist,
            int? pageSize,
            int? pageNumber,
            IMusicRepository repo
        )
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return Results.Ok(repo.GetAlbumsByName(name, pageSize, pageNumber));
            }

            if (!string.IsNullOrWhiteSpace(artist))
            {
                return Results.Ok(repo.GetAlbumsByArtist(artist, pageSize, pageNumber));
            }

            return Results.Ok(repo.GetAllAlbums(pageSize, pageNumber));
        }

        private static IResult GetAlbumById([FromQuery] Guid id, IMusicRepository repo)
        {
            var album = repo.GetAlbumById(id);
            return album is null ? Results.NotFound() : Results.Ok(album);
        }
    }
}
