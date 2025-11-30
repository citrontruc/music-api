/*
A class to store our albums in memory.
*/

namespace MusicDatabaseApi.Repositories
{
    using MusicDatabaseApi.Models;

    public class InMemoryMusicRepository : IMusicRepository
    {
        private readonly Dictionary<Guid, Album> _albums = new();
        private readonly object _lock = new();
        private AlbumParameters _defaultAlbumParameters;

        public InMemoryMusicRepository(AlbumParameters defaultAlbumParameters)
        {
            _defaultAlbumParameters = defaultAlbumParameters;
        }

        public Album CreateAlbum(CreateAlbumRequest request)
        {
            var album = new Album(
                Id: Guid.NewGuid(),
                Name: request.Name,
                ArtistName: request.ArtistName,
                ReleaseYear: request.ReleaseYear,
                Genre: request.Genre,
                CreatedAt: DateTime.UtcNow
            );

            lock (_lock)
            {
                _albums.Add(album.Id, album);
            }

            return album;
        }

        public IEnumerable<Album> GetAllAlbums(int number, int page)
        {
            int correctNumber =
                (_defaultAlbumParameters.PageSize > number)
                    ? number
                    : _defaultAlbumParameters.PageSize;
            int correctPage =
                (
                    _defaultAlbumParameters.PageNumber * _defaultAlbumParameters.PageSize
                    > page * correctNumber
                )
                    ? page
                    : _defaultAlbumParameters.PageNumber * _defaultAlbumParameters.PageSize
                        - correctNumber;
            lock (_lock)
            {
                return _albums
                    .Values.OrderBy(a => a.ArtistName)
                    .ThenBy(a => a.Name)
                    .Skip((correctPage - 1) * correctNumber)
                    .Take(correctNumber)
                    .ToList();
            }
        }

        public Album? GetAlbumById(Guid id)
        {
            lock (_lock)
            {
                _albums.TryGetValue(id, out var album);
                return album;
            }
        }

        public IEnumerable<Album> GetAlbumsByName(string name, int number, int page)
        {
            lock (_lock)
            {
                return _albums
                    .Values.Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(a => a.Name)
                    .ToList();
            }
        }

        public IEnumerable<Album> GetAlbumsByArtist(string artistName, int number, int page)
        {
            lock (_lock)
            {
                return _albums
                    .Values.Where(a =>
                        a.ArtistName.Contains(artistName, StringComparison.OrdinalIgnoreCase)
                    )
                    .OrderBy(a => a.ReleaseYear)
                    .ToList();
            }
        }
    }
}
