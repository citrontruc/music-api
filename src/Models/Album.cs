using System.ComponentModel.DataAnnotations;

namespace MusicDatabaseApi.Models
{
    public record Album(
        [Required] Guid Id,
        [StringLength(200)] string Name,
        string ArtistName,
        int ReleaseYear,
        string Genre,
        DateTime CreatedAt
    );
}
