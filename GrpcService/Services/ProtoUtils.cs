using Entities.plant;
using Google.Protobuf.WellKnownTypes;

namespace GrpcService.Services;

public static class ProtoUtils
{
    /// <summary>
    /// Maps a Plant entity to an optimal configuration PlantResponse.
    /// </summary>
    /// <param name="entity">The Plant entity to map.</param>
    /// <returns>A PlantResponse object representing the optimal configuration of the plant.</returns>
    public static PlantResponse MapToOptimalConfiguration(Plant entity)
    {
        return new PlantResponse
        {
            Username = entity.Username,
            OptimalTemperature = entity.OptimalTemperature,
            OptimalAirHumidity = entity.OptimalAirHumidity,
            OptimalSoilHumidity = entity.OptimalSoilHumidity,
            OptimalLightIntensity = entity.OptimalLightIntensity,
            TemperatureScale = (TemperatureScale)entity.Scale
        };
    }

    /// <summary>
    /// Maps a Plant entity to a PlantResponse object.
    /// </summary>
    /// <param name="entity">The Plant entity to map.</param>
    /// <returns>A PlantResponse object representing the mapped plant data.</returns>
    public static PlantResponse MapToPlantResponse(Plant entity)
    {
        var sensorDatas = entity.SensorDatas ?? [];
        var wateringDatas = entity.Waterings ?? [];

        var response = MapToOptimalConfiguration(entity);

        response.PlantMAC = entity.MAC.ToLower();
        response.Name = entity.Name;
        response.TemperatureScale = (TemperatureScale)entity.Scale;
        response.AddedDate = Timestamp.FromDateTime(entity.AddedDate.ToUniversalTime());
        response.ShouldPredictOptimal = entity.ShouldPredictOptimal;

        var latestSensor = sensorDatas
            .OrderByDescending(s => s.Id)
            .FirstOrDefault();

        if (latestSensor is not null)
        {
            response.SensorData = MapToSensorResponse(latestSensor);
        }

        response.PreviousSensorReadings ??= new PreviousSensorResponses();

        response.PreviousSensorReadings.Readings.AddRange(
            sensorDatas
                .OrderByDescending(s => s.Id)
                .Select(MapToSensorResponse)
        );

        var latestWatering = wateringDatas
            .OrderByDescending(w => w.Id)
            .FirstOrDefault();

        if (latestWatering is not null)
        {
            response.Watering = MapToWateringResponse(latestWatering);
        }

        response.PreviousWateringReadings ??= new PreviousWateringResponses();

        response.PreviousWateringReadings.Readings.AddRange(
            wateringDatas
                .OrderByDescending(w => w.Id)
                .Select(MapToWateringResponse)
        );

        return response;
    }
    public static SensorResponse? MapToSensorResponse(SensorData? sensor)
    {
        if (sensor == null)
        {
            return null;
        }
        return new SensorResponse
        {
            Id = sensor.Id,
            Temperature = sensor.Temperature,
            AirHumidity = sensor.AirHumidity,
            SoilHumidity = sensor.SoilHumidity,
            LightIntensity = sensor.LightIntensity,
            PlantMAC = sensor.PlantMAC.ToLower(),
            Timestamp = Timestamp.FromDateTime(
                sensor.Timestamp.ToUniversalTime()
            )
        };
    }

    public static WateringResponse? MapToWateringResponse(Watering? watering)
    {
        if (watering == null)
        {
            return null;
        }
        return new WateringResponse
        {
            Id = watering.Id,
            PumpTimeInSeconds = watering.PumpTimeInSeconds,
            WaterLevel = watering.WaterLevel,
            PlantMAC = watering.PlantMAC.ToLower(),
            LastWaterTime = Timestamp.FromDateTime(
                watering.LastWaterTime.ToUniversalTime()
            )
        };
    }
}