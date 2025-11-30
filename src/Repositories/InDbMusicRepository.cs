using Microsoft.EntityFrameworkCore;
using MusicDatabaseApi.Models;

namespace MusicDatabaseApi.Data;

public class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options)
        : base(options) { }

    public DbSet<Album> Albums { get; set; }
}
