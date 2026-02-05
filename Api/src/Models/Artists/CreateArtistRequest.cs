/*
Request to create an album in our database
*/

namespace MusicDatabaseApi.Models
{
    internal record CreateArtistRequest(string Name, string Genre) : ICreateMusicRequest;
}
