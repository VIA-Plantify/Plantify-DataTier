using Entities;
using Entities.plant;
using Microsoft.EntityFrameworkCore;

namespace EFC.DataAccess;

public class PlantifyContext : DbContext
{
    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<Plant> Plants => Set<Plant>();
    public virtual DbSet<Watering> Waterings => Set<Watering>();
    
    public virtual DbSet<SensorData> SensorDatas => Set<SensorData>();
    
    public PlantifyContext(DbContextOptions options) : base(options)
    {
    }
}