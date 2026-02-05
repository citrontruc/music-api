/*
A class to store our artist data.
*/

using System.ComponentModel.DataAnnotations;

namespace MusicDatabaseApi.Models
{
    internal record class Artist(
        [Required] Guid Id,
        [Required] [StringLength(100)] string Name,
        [Required] DateTime CreatedAt,
        [Required] bool IsDeleted
    );
}
