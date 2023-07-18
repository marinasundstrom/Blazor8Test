using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data;

public sealed class BlazorMovieContext : DbContext 
{
    public BlazorMovieContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; } = default!;
}