/*
A DbContext to store albums in a database
*/

using Microsoft.EntityFrameworkCore;
using MusicDatabaseApi.Models;

namespace MusicDatabaseApi.Data;

public class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options)
        : base(options) { }

    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
}
