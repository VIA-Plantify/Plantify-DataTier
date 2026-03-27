using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFC.DataAccess;

public class PlantifyContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public PlantifyContext(DbContextOptions options) : base(options)
    {
    }

    public PlantifyContext()
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=plantify;Username=dev;Password=plantifydev"
            );
        }
    }
}