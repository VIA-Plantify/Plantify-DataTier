using Entities.plant;

namespace RepositoryContracts;

public interface IPlantRepository
{
    Task<Plant> CreateAsync(string username,Plant plant);
    Task<Plant> GetPlantAsync(string username, int plantId);
    Task DeleteAsync(string username,int plantId);
    Task UpdateAsync(string username,Plant plant);
    IQueryable<Plant> GetMany(string username);
}