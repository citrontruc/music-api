/*
A class to store our albums in an SQL database.
*/
using ListPagination;
using Microsoft.EntityFrameworkCore;
using MusicDatabaseApi.Data;
using MusicDatabaseApi.Models;

namespace MusicDatabaseApi.Repositories
{
    public class SqlArtistRepository : IArtistRepository
    {
        private PaginationParameters _defaultPaginationParameters;

        public SqlArtistRepository(PaginationParameters defaultPaginationParameters)
        {
            _defaultPaginationParameters = defaultPaginationParameters;
        }

        #region Create requests
        public async Task<Artist> Create(MusicDbContext db, CreateArtistRequest request)
        {
            // Note: We save our changes but do not reload our database.
            var artist = new Artist(
                Id: Guid.NewGuid(),
                Name: request.Name,
                CreatedAt: DateTime.UtcNow,
                IsDeleted: false
            );
            await db.Artists.AddAsync(artist);
            db.SaveChanges();

            return artist;
        }
        #endregion

        #region Get requests
        public async Task<IEnumerable<Artist>> GetAll(
            MusicDbContext db,
            int? pageSize,
            int? pageNumber
        )
        {
            (int correctPageSize, int correctPageNumber) =
                PaginationParameters.CorrectPaginationParameters(
                    _defaultPaginationParameters,
                    pageSize,
                    pageNumber
                );

            return await PagedList<Artist>.ToPagedListAsync(
                db.Artists.AsNoTracking().OrderBy(a => a.Name),
                correctPageSize,
                correctPageNumber
            );
        }

        public async Task<Artist?> GetById(MusicDbContext db, Guid id)
        {
            Artist? artist = await db.Artists.FirstOrDefaultAsync(a => a.Id == id);
            ;
            return artist;
        }

        public async Task<IEnumerable<Artist>> GetByName(
            MusicDbContext db,
            string name,
            int? pageSize,
            int? pageNumber
        )
        {
            (int correctPageSize, int correctPageNumber) =
                PaginationParameters.CorrectPaginationParameters(
                    _defaultPaginationParameters,
                    pageSize,
                    pageNumber
                );
            return await PagedList<Artist>.ToPagedListAsync(
                db.Artists.Where(a => a.Name.ToLower().Contains(name.ToLower()))
                    .OrderBy(a => a.Name)
                    .AsNoTracking(),
                correctPageSize,
                correctPageNumber
            );
        }
        #endregion
    }
}
