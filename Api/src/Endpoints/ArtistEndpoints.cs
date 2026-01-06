/*
A class to add the endpoints to add artists to our database.
*/

using Asp.Versioning;
using Asp.Versioning.Builder;
using MusicDatabaseApi.Data;
using MusicDatabaseApi.Models;
using MusicDatabaseApi.Repositories;

namespace MusicDatabaseApi.Endpoints
{
    public static class ArtistEndpoints
    {
        #region Create Endpoints
        /// <summary>
        /// Extension of the app in order to add endpoints to retrieve albums
        /// </summary>
        /// <param name="app"></param>
        public static void MapArtistEndpoints(this WebApplication app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .HasApiVersion(new ApiVersion(2))
                .HasApiVersion(new ApiVersion(3))
                .ReportApiVersions()
                .Build();

            // We need to be authenticated in order to interact with the api.
            var group = app.MapGroup("api/v{version:apiVersion}/artists")
                .WithApiVersionSet(apiVersionSet)
                .WithTags("Artists")
                .HasApiVersion(1)
                .HasApiVersion(2)
                .HasApiVersion(3)
                .RequireAuthorization();

            group
                .MapPost("/", CreateArtist)
                .WithName("CreateArtist")
                .WithSummary("Create a new artist");

            group
                .MapGet("/", GetArtists)
                .WithName("GetArtists")
                .WithSummary("Get all artists or search by name");

            group
                .MapGet("/{id:guid}", GetArtistById)
                .WithName("GetArtistById")
                .WithSummary("Get a specific artist by ID")
                .WithDescription(
                    "Queries the artist with the corresponding id from the database and retrieves its information."
                );
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
