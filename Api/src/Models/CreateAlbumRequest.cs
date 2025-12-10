/*
Request to create an album in our database
*/

namespace MusicDatabaseApi.Models
{
    public record CreateAlbumRequest(string Name, string ArtistName, int ReleaseYear, string Genre);
}
