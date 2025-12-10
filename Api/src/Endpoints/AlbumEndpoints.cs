/*
A class to add the endpoints to add albums to our database.
*/

using MusicDatabaseApi.Data;
using MusicDatabaseApi.Models;
using MusicDatabaseApi.Repositories;

namespace MusicDatabaseApi.Endpoints
{
    public static class AlbumEndpoints
    {
        #region Create Endpoints
        /// <summary>
        /// Extension of the app in order to add endpoints to retrieve albums
        /// </summary>
        /// <param name="app"></param>
        public static void MapAlbumEndpoints(this WebApplication app)
        {
            // We need to be authenticated in order to interact with the api.
            var group = app.MapGroup("/api/albums").WithTags("Albums").RequireAuthorization();

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
        #endregion

        #region Resolve endpoints
        private static async Task<IResult> CreateAlbum(
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

            var album = await repo.CreateAlbum(db, request);
            return Results.Created($"/api/albums/{album.Id}", album);
        }

        private static async Task<IResult> GetAlbums(
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
                return Results.Ok(await repo.GetAlbumsByName(db, name, pageSize, pageNumber));
            }

            if (!string.IsNullOrWhiteSpace(artist))
            {
                return Results.Ok(await repo.GetAlbumsByArtist(db, artist, pageSize, pageNumber));
            }

            return Results.Ok(await repo.GetAllAlbums(db, pageSize, pageNumber));
        }

        private static async Task<IResult> GetAlbumById(
            MusicDbContext db,
            Guid id,
            IMusicRepository repo
        )
        {
            var album = await repo.GetAlbumById(db, id);
            return album is null ? Results.NotFound() : Results.Ok(album);
        }
        #endregion
    }
}
