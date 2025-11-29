namespace MusicDatabaseApi.Repositories
{
    using MusicDatabaseApi.Models;

    public interface IMusicRepository
    {
        Album CreateAlbum(CreateAlbumRequest request);
        IEnumerable<Album> GetAllAlbums();
        Album? GetAlbumById(Guid id);
        IEnumerable<Album> GetAlbumsByName(string name);
        IEnumerable<Album> GetAlbumsByArtist(string artistName);
    }
}
