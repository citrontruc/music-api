/*
A class to represent an album.
*/

using System.ComponentModel.DataAnnotations;

namespace MusicDatabaseApi.Models
{
    internal record class Album(
        [Required] Guid Id,
        [Required] [StringLength(200)] string Name,
        [Required] [StringLength(100)] string ArtistName,
        int ReleaseYear,
        string Genre,
        [Required] DateTime CreatedAt,
        [Required] bool IsDeleted
    );
}
