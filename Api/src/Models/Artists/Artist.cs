/*
A class to store our artist data.
*/

using System.ComponentModel.DataAnnotations;

namespace MusicDatabaseApi.Models
{
    public record class Artist(
        [Required] Guid Id,
        [Required] [StringLength(100)] string Name,
        [Required] bool IsDeleted
    );
}
