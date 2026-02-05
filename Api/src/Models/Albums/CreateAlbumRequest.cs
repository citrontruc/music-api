/*
Request to create an album in our database
*/

namespace MusicDatabaseApi.Models
{
    internal record CreateAlbumRequest(
        string Name,
        string ArtistName,
        int ReleaseYear,
        string Genre
    ) : ICreateMusicRequest;
}
