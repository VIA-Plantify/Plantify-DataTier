using EFC.DataAccess;
using Entities.plant;
using RepositoryContracts;

namespace EFC.Repositories;

/// <summary>
/// Repository class for managing soil humidity data.
/// Implements the ISoilHumidityRepository interface to provide asynchronous operations for creating soil humidity records.
/// </summary>
public class SoilHumidityRepository(PlantifyContext context) : ISoilHumidityRepository
{
    /// <summary>
    /// Asynchronously creates a new soil humidity record in the repository.
    /// </summary>
    /// <param name="plantMAC">The MAC address of the plant associated with the soil humidity data.</param>
    /// <param name="soilHumidity">The SoilHumidity object containing the soil humidity data to be created.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public Task CreateAsync(string plantMAC, SoilHumidity soilHumidity)
    {
        soilHumidity.PlantMAC = plantMAC;
        context.SoilHumidity.Add(soilHumidity);
        return context.SaveChangesAsync();
    }
}