using Entities.plant;

namespace RepositoryContracts;

/// <summary>
/// Repository interface for managing soil humidity data.
/// Provides asynchronous operations to create soil humidity records associated with a plant identified by its MAC address.
/// </summary>
public interface ISoilHumidityRepository
{
    /// <summary>
    /// Asynchronously creates a new soil humidity record in the repository.
    /// </summary>
    /// <param name="plantMAC">The MAC address of the plant associated with the soil humidity data.</param>
    /// <param name="soilHumidity">The SoilHumidity object containing the soil humidity data to be created.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task CreateAsync(string plantMAC,SoilHumidity soilHumidity);
}