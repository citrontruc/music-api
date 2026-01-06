/*
Request to create an album in our database
*/

namespace MusicDatabaseApi.Models
{
    public record CreateArtistRequest(string Name, string Genre) : ICreateMusicRequest;
}
