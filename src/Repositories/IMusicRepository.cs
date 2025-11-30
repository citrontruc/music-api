namespace MusicDatabaseApi.Repositories
{
    using MusicDatabaseApi.Models;

    public interface IMusicRepository
    {
        Album CreateAlbum(CreateAlbumRequest request);
        IEnumerable<Album> GetAllAlbums(int? pageSize, int? pageNumber);
        Album? GetAlbumById(Guid id);
        IEnumerable<Album> GetAlbumsByName(string name, int? pageSize, int? pageNumber);
        IEnumerable<Album> GetAlbumsByArtist(string artistName, int? pageSize, int? pageNumber);
    }
}
