using EFC.DataAccess;
using Entities.plant;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EFC.Repositories;

/// <summary>
/// Repository class for managing plant entities within the application.
/// Implements the IPlantRepository interface to provide CRUD operations for plants associated with a specific user.
/// </summary>
public class PlantRepository(PlantifyContext context) : IPlantRepository
{
    /// <summary>
    /// Represents the database context for Plantify application, providing access to various entity sets including Plants.
    /// </summary>
    private readonly PlantifyContext context = context;

    /// <summary>
    /// Creates a new plant asynchronously.
    /// </summary>
    /// <param name="plant">The plant entity to be created.</param>
    /// <returns>A task representing the asynchronous operation that returns the created Plant object.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a plant with the same MAC address already exists for the given user.</exception>
    public async Task<Plant> CreateAsync(Plant plant)
    {
        var existingPlant =
            await context.Plants.FirstOrDefaultAsync(p => p.Username == plant.Username && p.MAC == plant.MAC);
        if (existingPlant != null)
        {
            throw new InvalidOperationException($"Plant with MAC {plant.MAC} already exists.");
        }

        context.Plants.Add(plant);
        await context.SaveChangesAsync();
        return plant;
    }

    /// <summary>
    /// Retrieves a plant asynchronously based on the specified username and plant MAC address.
    /// </summary>
    /// <param name="username">The username of the user owning the plant.</param>
    /// <param name="plantMAC">The MAC address of the plant to retrieve.</param>
    /// <param name="numberOfSensorReadings">Optional. The number of latest sensor readings to include. Defaults to 10 if not provided or zero.</param>
    /// <param name="numberOfWateringReadings">Optional. The number of latest watering records to include. Defaults to 1 if not provided or zero.</param>
    /// <returns>A task representing the asynchronous operation that returns the retrieved Plant object.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a plant with the specified MAC address for the given user is not found.</exception>
    public async Task<Plant> GetPlantAsync(string username, string plantMAC, int? numberOfSensorReadings, int? numberOfWateringReadings)
    {
        var take = numberOfSensorReadings is null or 0 ? 10 : numberOfSensorReadings.Value;
        var wateringTake = numberOfWateringReadings is null or 0 ? 1 : numberOfWateringReadings.Value;
        
        var plant = await context.Plants.Where(p =>  p.Username == username && p.MAC == plantMAC).Select(p=>
            new Plant()
            {
                MAC = p.MAC,
                Name = p.Name,
                Username = p.Username,
                Scale = p.Scale,
                OptimalTemperature = p.OptimalTemperature,
                OptimalAirHumidity = p.OptimalAirHumidity,
                OptimalLightIntensity = p.OptimalLightIntensity,
                OptimalSoilHumidity = p.OptimalSoilHumidity,
                
                SensorDatas = p.SensorDatas
                    .OrderByDescending(r => r.Id)
                    .Take(take)
                    .ToList(),

                Waterings = p.Waterings
                    .OrderByDescending(r => r.Id)
                    .Take(wateringTake)
                    .ToList()
            }).FirstOrDefaultAsync();
        
        if (plant == null)
        {
            throw new InvalidOperationException($"Plant with MAC '{plantMAC}' for user '{username}' not found.");
        }

        return plant;
    }

    /// <summary>
    /// Deletes a plant associated with the given username and MAC address.
    /// </summary>
    /// <param name="username">The username of the user who owns the plant.</param>
    /// <param name="plantMAC">The MAC address of the plant to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteAsync(string username, string plantMAC)
    {
        var plant = await context.Plants.FirstOrDefaultAsync(p => p.Username == username && p.MAC == plantMAC);
        if (plant != null)
        {
            context.Plants.Remove(plant);
            await context.SaveChangesAsync();
        }
    }


    /// <summary>
    /// Updates an existing plant asynchronously.
    /// </summary>
    /// <param name="plant">The updated plant entity.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the specified plant does not exist for the given user.</exception>
    public async Task UpdateAsync(Plant plant)
    {
        var existingPlant =
            await context.Plants.FirstOrDefaultAsync(p => p.Username == plant.Username && p.MAC == plant.MAC);
        if (existingPlant == null)
        {
            throw new InvalidOperationException($"Plant with MAC '{plant.MAC}' for user '{plant.Username}' not found.");
        }

        existingPlant.Name = plant.Name;
        existingPlant.OptimalTemperature = plant.OptimalTemperature;
        existingPlant.OptimalAirHumidity = plant.OptimalAirHumidity;
        existingPlant.OptimalSoilHumidity = plant.OptimalSoilHumidity;
        existingPlant.OptimalLightIntensity = plant.OptimalLightIntensity;

        context.Plants.Update(existingPlant);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves multiple plants associated with a given username.
    /// </summary>
    /// <param name="username">The username of the owner of the plants.</param>
    /// <returns>An IQueryable collection of Plant objects that match the given username.</returns>
    public IQueryable<Plant> GetMany(string username, int? numberOfSensorReadings, int? numberOfWateringReadings)
    {
        var take = numberOfSensorReadings is null or 0 ? 10 : numberOfSensorReadings.Value;
        var wateringTake = numberOfWateringReadings is null or 0 ? 1 : numberOfWateringReadings.Value;

        return context.Plants.Where(p => p.Username == username).Select(p => new Plant
            {
                MAC = p.MAC,
                Username = p.Username,
                Name = p.Name,
                Scale = p.Scale,
                OptimalTemperature = p.OptimalTemperature,
                OptimalAirHumidity = p.OptimalAirHumidity,
                OptimalSoilHumidity = p.OptimalSoilHumidity,
                OptimalLightIntensity = p.OptimalLightIntensity,
                
                SensorDatas = p.SensorDatas.Take(take).ToList(),
                Waterings = p.Waterings.Take(wateringTake).ToList()
            });
    }
}