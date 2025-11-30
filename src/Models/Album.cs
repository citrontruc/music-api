using System.ComponentModel.DataAnnotations;

namespace MusicDatabaseApi.Models
{
    public record Album(
        [Required] Guid Id,
        [Required] [StringLength(200)] string Name,
        [Required] [StringLength(100)] string ArtistName,
        int ReleaseYear,
        string Genre,
        [Required] DateTime CreatedAt
    );
}
