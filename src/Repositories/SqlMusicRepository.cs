/*
A class to store our albums in memory.
*/
using Microsoft.EntityFrameworkCore;

namespace MusicDatabaseApi.Repositories
{
    using MusicDatabaseApi.Data;
    using MusicDatabaseApi.Models;

    public class SqlMusicRepository : IMusicRepository
    {
        private AlbumParameters _defaultAlbumParameters;

        public SqlMusicRepository(AlbumParameters defaultAlbumParameters)
        {
            _defaultAlbumParameters = defaultAlbumParameters;
        }

        public Album CreateAlbum(MusicDbContext db, CreateAlbumRequest request)
        {
            // Note: We save our changes but do not reload our database.
            var album = new Album(
                Id: Guid.NewGuid(),
                Name: request.Name,
                ArtistName: request.ArtistName,
                ReleaseYear: request.ReleaseYear,
                Genre: request.Genre,
                CreatedAt: DateTime.UtcNow
            );
            db.Albums.Add(album);
            db.SaveChanges();

            return album;
        }

        public IEnumerable<Album> GetAllAlbums(MusicDbContext db, int? pageSize, int? pageNumber)
        {
            (int correctPageSize, int correctPageNumber) = CorrectPaginationParameters(
                pageSize,
                pageNumber
            );

            return PagedList<Album>.ToPagedList(
                db.Albums.AsNoTracking().OrderBy(a => a.ArtistName).ThenBy(a => a.Name),
                correctPageSize,
                correctPageNumber
            );
        }

        public Album? GetAlbumById(MusicDbContext db, Guid id)
        {
            Album? album = db.Albums.FirstOrDefault(a => a.Id == id);
            ;
            return album;
        }

        public IEnumerable<Album> GetAlbumsByName(
            MusicDbContext db,
            string name,
            int? pageSize,
            int? pageNumber
        )
        {
            (int correctPageSize, int correctPageNumber) = CorrectPaginationParameters(
                pageSize,
                pageNumber
            );
            return PagedList<Album>.ToPagedList(
                db.Albums.Where(a => a.Name.ToLower().Contains(name.ToLower()))
                    .OrderBy(a => a.Name)
                    .AsNoTracking(),
                correctPageSize,
                correctPageNumber
            );
        }

        public IEnumerable<Album> GetAlbumsByArtist(
            MusicDbContext db,
            string artistName,
            int? pageSize,
            int? pageNumber
        )
        {
            (int correctPageSize, int correctPageNumber) = CorrectPaginationParameters(
                pageSize,
                pageNumber
            );
            return PagedList<Album>.ToPagedList(
                db.Albums.Where(a => a.ArtistName.ToLower().Contains(artistName.ToLower()))
                    .OrderBy(a => a.ArtistName)
                    .AsNoTracking(),
                correctPageSize,
                correctPageNumber
            );
        }

        private (int, int) CorrectPaginationParameters(int? pageSize, int? pageNumber)
        {
            int interPageSize = pageSize ?? _defaultAlbumParameters.PageSize;
            int interPageNumber = pageNumber ?? _defaultAlbumParameters.PageNumber;
            int correctPageSize =
                (_defaultAlbumParameters.PageSize >= interPageSize)
                    ? interPageSize
                    : _defaultAlbumParameters.PageSize;
            int correctPageNumber =
                (
                    _defaultAlbumParameters.PageNumber * _defaultAlbumParameters.PageSize
                    >= interPageNumber * correctPageSize
                )
                    ? interPageNumber
                    : _defaultAlbumParameters.PageNumber * _defaultAlbumParameters.PageSize
                        - correctPageSize;
            return (correctPageSize, correctPageNumber);
        }
    }
}
