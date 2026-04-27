using Entities.plant;

namespace RepositoryContracts;

public interface IPlantRepository
{
    Task<Plant> CreateAsync(Plant plant);
    Task<Plant> GetPlantAsync(string username, string plantMAC);
    Task DeleteAsync(string username,string plantMAC);
    Task UpdateAsync(Plant plant);
    IQueryable<Plant> GetMany(string username);
}