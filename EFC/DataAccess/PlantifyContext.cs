using Entities;
using Entities.plant;
using Microsoft.EntityFrameworkCore;

namespace EFC.DataAccess;

public class PlantifyContext : DbContext
{
    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<Plant> Plants => Set<Plant>();
    public virtual DbSet<Temperature> Temperatures => Set<Temperature>();
    public virtual DbSet<WaterIntake> WaterIntakes => Set<WaterIntake>();
    public virtual DbSet<SoilHumidity> SoilHumidity => Set<SoilHumidity>();
    public virtual DbSet<AirHumidity> AirHumidity => Set<AirHumidity>();
    public virtual DbSet<LightIntensity> LightIntensities => Set<LightIntensity>();
    
    public PlantifyContext(DbContextOptions options) : base(options)
    {
    }
}