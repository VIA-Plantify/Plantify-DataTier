using Entities.plant;

namespace RepositoryContracts;

public interface  IWateringRepository
{
    Task CreateWatering(Watering watering);
    Task GetWatering(string plantMac);
}