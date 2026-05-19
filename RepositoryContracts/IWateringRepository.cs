using Entities.plant;

namespace RepositoryContracts;

public interface  IWateringRepository
{
    Task CreateWatering(Watering watering);
    Task<Watering?> GetWateringAsync(string plantMac);
    Task<Watering?> GetLastWithPumpTimeAsync(string plantMac);
}