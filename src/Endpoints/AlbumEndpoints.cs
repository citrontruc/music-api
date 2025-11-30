/*
A class to add the endpoints to add albums to our database.
*/

using MusicDatabaseApi.Data;

namespace MusicDatabaseApi.Endpoints
{
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

        private static IResult CreateAlbum(
            MusicDbContext db,
            CreateAlbumRequest request,
            IMusicRepository repo
        )
        {
            if (
                string.IsNullOrWhiteSpace(request.Name)
                || string.IsNullOrWhiteSpace(request.ArtistName)
            )
            {
                return Results.BadRequest("Album name and artist name are required.");
            }

            var album = repo.CreateAlbum(db, request);
            return Results.Created($"/api/albums/{album.Id}", album);
        }

        private static IResult GetAlbums(
            MusicDbContext db,
            string? name,
            string? artist,
            int? pageSize,
            int? pageNumber,
            IMusicRepository repo
        )
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return Results.Ok(repo.GetAlbumsByName(db, name, pageSize, pageNumber));
            }

            if (!string.IsNullOrWhiteSpace(artist))
            {
                return Results.Ok(repo.GetAlbumsByArtist(db, artist, pageSize, pageNumber));
            }

            return Results.Ok(repo.GetAllAlbums(db, pageSize, pageNumber));
        }

        private static IResult GetAlbumById(MusicDbContext db, Guid id, IMusicRepository repo)
        {
            var album = repo.GetAlbumById(db, id);
            return album is null ? Results.NotFound() : Results.Ok(album);
        }
    }
}
