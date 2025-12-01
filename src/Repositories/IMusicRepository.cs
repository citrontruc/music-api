using MusicDatabaseApi.Data;

namespace MusicDatabaseApi.Repositories
{
    using MusicDatabaseApi.Models;

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
