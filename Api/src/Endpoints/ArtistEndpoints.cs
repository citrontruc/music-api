/*
A class to add the endpoints to add artists to our database.
*/

using MusicDatabaseApi.Data;
using MusicDatabaseApi.Models;
using MusicDatabaseApi.Repositories;

namespace MusicDatabaseApi.Endpoints
{
    internal static class ArtistEndpoints
    {
        #region Create Endpoints
        /// <summary>
        /// Extension of the app in order to add endpoints to retrieve albums
        /// </summary>
        /// <param name="app"></param>
        public static void MapArtistEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("artists/", CreateArtist)
                .WithName("CreateArtist")
                .WithSummary("Create a new artist")
                .MapToApiVersion(2)
                .MapToApiVersion(3);

            app.MapGet("artists/", GetArtists)
                .WithName("GetArtists")
                .WithSummary("Get all artists or search by name")
                .MapToApiVersion(2)
                .MapToApiVersion(3);

            app.MapGet("artists/{id:guid}", GetArtistById)
                .WithName("GetArtistById")
                .WithSummary("Get a specific artist by ID")
                .WithDescription(
                    "Queries the artist with the corresponding id from the database and retrieves its information."
                )
                .MapToApiVersion(2)
                .MapToApiVersion(3);
        }
        #endregion

        #region Resolve endpoints
        private static async Task<IResult> CreateArtist(
            MusicDbContext db,
            CreateArtistRequest request,
            IArtistRepository repo
        )
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest("Artist name is required.");
            }

            var artist = await repo.Create(db, request);
            return Results.Created($"/api/artists/{artist.Id}", artist);
        }

        private static async Task<IResult> GetArtists(
            MusicDbContext db,
            string? name,
            int? pageSize,
            int? pageNumber,
            IArtistRepository repo
        )
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return Results.Ok(await repo.GetByName(db, name, pageSize, pageNumber));
            }
            return Results.Ok(await repo.GetAll(db, pageSize, pageNumber));
        }

        private static async Task<IResult> GetArtistById(
            MusicDbContext db,
            Guid id,
            IArtistRepository repo
        )
        {
            var artist = await repo.GetById(db, id);
            return artist is null ? Results.NotFound() : Results.Ok(artist);
        }
        #endregion
    }
}
