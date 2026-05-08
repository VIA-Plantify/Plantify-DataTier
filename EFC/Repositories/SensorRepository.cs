using EFC.DataAccess;
using Entities.plant;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EFC.Repositories;

/// <summary>
/// Repository class for managing sensor data related to plants.
/// Implements the ISensorRepository interface to provide asynchronous operations for creating and retrieving sensor data.
/// </summary>
public class SensorRepository(PlantifyContext context) : ISensorRepository
{
    /// <summary>
    /// Creates sensor data in the database.
    /// </summary>
    /// <param name="sensorData">The SensorData object to be created.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task CreateSensorData(SensorData sensorData)
    {
        await context.SensorDatas.AddAsync(sensorData);
        await context.SaveChangesAsync();
        await Task.CompletedTask;
    }

    /// <summary>
    /// Retrieves the latest sensor data for a specific plant.
    /// </summary>
    /// <param name="plantMac">The MAC address of the plant.</param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains the latest SensorData object or null if no data is found.</returns>
    public async Task<SensorData?> GetLatestAsync(string plantMac)
    {
        return await context.SensorDatas.OrderByDescending(s => s.Id).FirstOrDefaultAsync(s => s.PlantMAC ==  plantMac);
    }
}