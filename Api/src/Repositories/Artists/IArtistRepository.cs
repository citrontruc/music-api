/*
An interface to implement the repository pattern for our albums.
*/
using MusicDatabaseApi.Data;
using MusicDatabaseApi.Models;

namespace MusicDatabaseApi.Repositories
{
    public interface IArtistRepository
    {
        Task<Artist> Create(MusicDbContext db, CreateArtistRequest request);
        Task<IEnumerable<Artist>> GetAll(MusicDbContext db, int? pageSize, int? pageNumber);
        Task<Artist?> GetById(MusicDbContext db, Guid id);
        Task<IEnumerable<Artist>> GetByName(
            MusicDbContext db,
            string name,
            int? pageSize,
            int? pageNumber
        );
    }
}
