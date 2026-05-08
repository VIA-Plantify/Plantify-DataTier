using Entities.plant;

namespace RepositoryContracts;

/// <summary>
/// Repository interface for managing sensor data related to plants.
/// </summary>
public interface ISensorRepository
{
    /// <summary>
    /// Creates sensor data in the database.
    /// </summary>
    /// <param name="sensorData">The SensorData object to be created.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task CreateSensorData(SensorData sensorData);

    /// <summary>
    /// Retrieves the latest sensor data for a specific plant.
    /// </summary>
    /// <param name="plantMac">The MAC address of the plant.</param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains the latest SensorData object or null if no data is found.</returns>
    Task<SensorData?> GetLatestAsync(string plantMac);
}