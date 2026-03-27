using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFC.DataAccess;

public class PlantifyContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public PlantifyContext(DbContextOptions options) : base(options)
    {
    }
}