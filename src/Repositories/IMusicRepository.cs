/*
An interface to implement the repository pattern for our albums.
*/
using MusicDatabaseApi.Data;
using MusicDatabaseApi.Models;

namespace MusicDatabaseApi.Repositories
{
    public interface IMusicRepository
    {
        Task<Album> CreateAlbum(MusicDbContext db, CreateAlbumRequest request);
        Task<IEnumerable<Album>> GetAllAlbums(MusicDbContext db, int? pageSize, int? pageNumber);
        Task<Album?> GetAlbumById(MusicDbContext db, Guid id);
        Task<IEnumerable<Album>> GetAlbumsByName(
            MusicDbContext db,
            string name,
            int? pageSize,
            int? pageNumber
        );
        Task<IEnumerable<Album>> GetAlbumsByArtist(
            MusicDbContext db,
            string artistName,
            int? pageSize,
            int? pageNumber
        );
    }
}
