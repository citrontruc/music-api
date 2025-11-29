namespace MusicDatabaseApi.Models
{
    public record Album(
        Guid Id,
        string Name,
        string ArtistName,
        int ReleaseYear,
        string Genre,
        DateTime CreatedAt
    );
}
