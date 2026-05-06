using Entities.plant;

namespace RepositoryContracts;

public interface IPlantRepository
{
    Task<Plant> CreateAsync(Plant plant);
    Task<Plant> GetPlantAsync(string username, string plantMAC, int? numberOfSensorReadings, int? numberOfWateringReadings);
    Task DeleteAsync(string username,string plantMAC);
    Task UpdateAsync(Plant plant);
    IQueryable<Plant> GetMany(string username, int? numberOfSensorReadings, int? numberOfWateringReadings);
    IQueryable<Plant> GetAllPlants();
}