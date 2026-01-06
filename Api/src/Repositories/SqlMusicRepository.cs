/*
A class to store our albums in an SQL database.
*/
using ListPagination;
using Microsoft.EntityFrameworkCore;
using MusicDatabaseApi.Data;
using MusicDatabaseApi.Models;

namespace MusicDatabaseApi.Repositories
{
    public class SqlMusicRepository : IMusicRepository
    {
        private AlbumParameters _defaultAlbumParameters;

        public SqlMusicRepository(AlbumParameters defaultAlbumParameters)
        {
            _defaultAlbumParameters = defaultAlbumParameters;
        }

        #region Create requests
        public async Task<Album> CreateAlbum(MusicDbContext db, CreateAlbumRequest request)
        {
            // Note: We save our changes but do not reload our database.
            var album = new Album(
                Id: Guid.NewGuid(),
                Name: request.Name,
                ArtistName: request.ArtistName,
                ReleaseYear: request.ReleaseYear,
                Genre: request.Genre,
                CreatedAt: DateTime.UtcNow,
                IsDeleted: false
            );
            await db.Albums.AddAsync(album);
            db.SaveChanges();

            return album;
        }
        #endregion

        #region Get requests
        public async Task<IEnumerable<Album>> GetAllAlbums(
            MusicDbContext db,
            int? pageSize,
            int? pageNumber
        )
        {
            (int correctPageSize, int correctPageNumber) =
                AlbumParameters.CorrectPaginationParameters(
                    _defaultAlbumParameters,
                    pageSize,
                    pageNumber
                );

            return await PagedList<Album>.ToPagedListAsync(
                db.Albums.AsNoTracking().OrderBy(a => a.ArtistName).ThenBy(a => a.Name),
                correctPageSize,
                correctPageNumber
            );
        }

        public async Task<Album?> GetAlbumById(MusicDbContext db, Guid id)
        {
            Album? album = await db.Albums.FirstOrDefaultAsync(a => a.Id == id);
            ;
            return album;
        }

        public async Task<IEnumerable<Album>> GetAlbumsByName(
            MusicDbContext db,
            string name,
            int? pageSize,
            int? pageNumber
        )
        {
            (int correctPageSize, int correctPageNumber) =
                AlbumParameters.CorrectPaginationParameters(
                    _defaultAlbumParameters,
                    pageSize,
                    pageNumber
                );
            return await PagedList<Album>.ToPagedListAsync(
                db.Albums.Where(a => a.Name.ToLower().Contains(name.ToLower()))
                    .OrderBy(a => a.Name)
                    .AsNoTracking(),
                correctPageSize,
                correctPageNumber
            );
        }

        public async Task<IEnumerable<Album>> GetAlbumsByArtist(
            MusicDbContext db,
            string artistName,
            int? pageSize,
            int? pageNumber
        )
        {
            (int correctPageSize, int correctPageNumber) =
                AlbumParameters.CorrectPaginationParameters(
                    _defaultAlbumParameters,
                    pageSize,
                    pageNumber
                );
            return await PagedList<Album>.ToPagedListAsync(
                db.Albums.Where(a => a.ArtistName.ToLower().Contains(artistName.ToLower()))
                    .OrderBy(a => a.ArtistName)
                    .AsNoTracking(),
                correctPageSize,
                correctPageNumber
            );
        }
        #endregion
    }
}
