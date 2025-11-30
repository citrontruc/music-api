using MusicDatabaseApi.Data;

namespace MusicDatabaseApi.Repositories
{
    using MusicDatabaseApi.Models;

    public interface IMusicRepository
    {
        Album CreateAlbum(MusicDbContext db, CreateAlbumRequest request);
        IEnumerable<Album> GetAllAlbums(MusicDbContext db, int? pageSize, int? pageNumber);
        Album? GetAlbumById(MusicDbContext db, Guid id);
        IEnumerable<Album> GetAlbumsByName(
            MusicDbContext db,
            string name,
            int? pageSize,
            int? pageNumber
        );
        IEnumerable<Album> GetAlbumsByArtist(
            MusicDbContext db,
            string artistName,
            int? pageSize,
            int? pageNumber
        );
    }
}
