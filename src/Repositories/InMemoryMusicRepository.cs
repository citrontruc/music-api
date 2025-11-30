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

        public IEnumerable<Album> GetAllAlbums(int? pageSize, int? pageNumber)
        {
            (int correctPageSize, int correctPageNumber) = CorrectPaginationParameters(pageSize, pageNumber);
            lock (_lock)
            {
                return PagedList<Album>.ToPagedList(_albums
                    .Values.OrderBy(a => a.ArtistName)
                    .ThenBy(a => a.Name)
                    .Skip((correctPageNumber - 1) * correctPageSize)
                    .Take(correctPageSize)
                    .ToList(), correctPageSize, correctPageNumber);
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

        public IEnumerable<Album> GetAlbumsByName(string name, int? pageSize, int? pageNumber)
        {
            (int correctPageSize, int correctPageNumber) = CorrectPaginationParameters(pageSize, pageNumber);
            lock (_lock)
            {
                return _albums
                    .Values.Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .Skip((correctPageNumber - 1) * correctPageSize)
                    .Take(correctPageSize)
                    .OrderBy(a => a.Name)
                    .ToList();
            }
        }

        public IEnumerable<Album> GetAlbumsByArtist(string artistName, int? pageSize, int? pageNumber)
        {
            (int correctPageSize, int correctPageNumber) = CorrectPaginationParameters(pageSize, pageNumber);
            lock (_lock)
            {
                return _albums
                    .Values.Where(a =>
                        a.ArtistName.Contains(artistName, StringComparison.OrdinalIgnoreCase)
                    )
                    .Skip((correctPageNumber - 1) * correctPageSize)
                    .Take(correctPageSize)
                    .OrderBy(a => a.ReleaseYear)
                    .ToList();
            }
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
