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
        public static void MapAlbumEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("albums/", CreateAlbum)
                .WithName("CreateAlbum")
                .WithSummary("Create a new album")
                .MapToApiVersion(1)
                .MapToApiVersion(2)
                .MapToApiVersion(3);

            app.MapGet("albums/", GetAlbums)
                .WithName("GetAlbums")
                .WithSummary("Get all albums or search by name/artist")
                .MapToApiVersion(2)
                .MapToApiVersion(3);

            app.MapGet("albums/{id:guid}", GetAlbumById)
                .WithName("GetAlbumById")
                .WithSummary("Get a specific album by ID")
                .WithDescription(
                    "Queries the album with the corresponding id from the database and retrieves its information."
                )
                .MapToApiVersion(3);

            app.MapDelete("albums/{id:guid}", DeleteAlbum)
                .WithName("Delete Album")
                .WithSummary("Delete an album given its ID.")
                .MapToApiVersion(3);
        }
        #endregion

        #region Resolve endpoints
        private static async Task<IResult> CreateAlbum(
            MusicDbContext db,
            CreateAlbumRequest request,
            IAlbumRepository repo
        )
        {
            if (
                string.IsNullOrWhiteSpace(request.Name)
                || string.IsNullOrWhiteSpace(request.ArtistName)
            )
            {
                return Results.BadRequest("Album name and artist name are required.");
            }

            var album = await repo.Create(db, request);
            return Results.Created($"/api/albums/{album.Id}", album);
        }

        private static async Task<IResult> GetAlbums(
            MusicDbContext db,
            string? name,
            string? artist,
            int? pageSize,
            int? pageNumber,
            IAlbumRepository repo
        )
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return Results.Ok(await repo.GetByName(db, name, pageSize, pageNumber));
            }

            if (!string.IsNullOrWhiteSpace(artist))
            {
                return Results.Ok(await repo.GetByArtist(db, artist, pageSize, pageNumber));
            }

            return Results.Ok(await repo.GetAll(db, pageSize, pageNumber));
        }

        private static async Task<IResult> GetAlbumById(
            MusicDbContext db,
            Guid id,
            IAlbumRepository repo
        )
        {
            var album = await repo.GetById(db, id);
            return album is null ? Results.NotFound() : Results.Ok(album);
        }

        private static async Task<IResult> DeleteAlbum(
            MusicDbContext db,
            Guid id,
            IAlbumRepository repo
        )
        {
            var (album, success) = await repo.Delete(db, id);
            if (album is null)
            {
                return Results.NoContent();
            }
            return success == 0
                ? Results.InternalServerError(
                    new DeletionFailedException($"Could not delete album with ID {id}")
                )
                : Results.Ok(album);
        }
        #endregion
    }
}
