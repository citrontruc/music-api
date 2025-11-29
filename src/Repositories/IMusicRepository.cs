namespace MusicDatabaseApi.Repositories
{
    using MusicDatabaseApi.Models;

    public interface IMusicRepository
    {
        Album CreateAlbum(CreateAlbumRequest request);
        IEnumerable<Album> GetAllAlbums(int number, int page);
        Album? GetAlbumById(Guid id);
        IEnumerable<Album> GetAlbumsByName(string name, int number, int page);
        IEnumerable<Album> GetAlbumsByArtist(string artistName, int number, int page);
    }
}
