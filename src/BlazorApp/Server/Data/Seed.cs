
using BlazorApp.Data;

namespace BlazorApp;

public static class Seed
{
    public static Task SeedData(ApplicationDbContext db, IConfiguration configuration)
    {
        return Task.CompletedTask;
    }
}
