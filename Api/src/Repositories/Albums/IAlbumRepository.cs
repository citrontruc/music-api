/*
An interface to implement the repository pattern for our albums.
*/
using MusicDatabaseApi.Data;
using MusicDatabaseApi.Models;

namespace MusicDatabaseApi.Repositories
{
    public interface IAlbumRepository
    {
        Task<Album> Create(MusicDbContext db, CreateAlbumRequest request);
        Task<IEnumerable<Album>> GetAll(MusicDbContext db, int? pageSize, int? pageNumber);
        Task<Album?> GetById(MusicDbContext db, Guid id);
        Task<IEnumerable<Album>> GetByName(
            MusicDbContext db,
            string name,
            int? pageSize,
            int? pageNumber
        );
        Task<IEnumerable<Album>> GetByArtist(
            MusicDbContext db,
            string artistName,
            int? pageSize,
            int? pageNumber
        );
        Task<(Album?, int)> Delete(MusicDbContext db, Guid id);
    }
}
