/*
A DbContext to store albums in a database
*/

using Microsoft.EntityFrameworkCore;
using MusicDatabaseApi.Models;

namespace MusicDatabaseApi.Data;

internal class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options)
        : base(options) { }

    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Filtering for soft delete
        modelBuilder.Entity<Album>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Artist>().HasQueryFilter(p => !p.IsDeleted);
    }
}
