using Entities.plant;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace GrpcService.Services;

/// <summary>
/// Provides gRPC services for managing plants.
/// This class implements the PlantServiceProtoBase and handles operations such as creating, deleting,
/// retrieving, and updating plant records through gRPC methods. It interacts with a repository to perform
/// data operations.
/// </summary>
public class PlantService(IPlantRepository repository) : PlantServiceProto.PlantServiceProtoBase
{
    /// <summary>
    /// Creates a new plant entity from the provided request.
    /// </summary>
    /// <param name="request">The CreatePlantRequest containing the details of the plant to be created.</param>
    /// <param name="context">The context of the server call.</param>
    /// <returns>A PlantResponse object representing the newly created plant.</returns>
    public async override Task<PlantResponse> Create(CreatePlantRequest request, ServerCallContext context)
    {
        var plant = new Plant
        {
            Username = request.Username,
            Name = request.Name,
    
            OptimalTemperature = request.OptimalTemperature,
            OptimalAirHumidity = request.OptimalAirHumidity,
            OptimalSoilHumidity = request.OptimalSoilHumidity,
            OptimalLightIntensity = request.OptimalLightIntensity,
            Scale = (Entities.plant.TemperatureScale)(int)request.TemperatureScale
        };
    
        try
        {
            var createdPlant = await repository.CreateAsync(plant);
            return MapToPlantResponse(createdPlant, null);
        }
        catch (InvalidOperationException ex)
        {
            // Plant already exists or other repository errors
            throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
        }
    }

    /// <summary>
    /// Deletes a plant entity associated with the specified user and MAC address.
    /// </summary>
    /// <param name="request">The DeletePlantRequest containing the username and PlantMAC of the plant to delete.</param>
    /// <returns>A Task that completes when the deletion operation is finished. The return type is Empty, indicating no value is returned.</returns>
    public async override Task<Empty> Delete(DeletePlantRequest request, ServerCallContext context)
    {
            await repository.DeleteAsync(request.Username, request.PlantMAC);
            return new Empty();
    }

    /// <summary>
    /// Retrieves a plant by username and MAC address.
    /// </summary>
    /// <param name="request">The request containing the username and MAC address of the plant.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>A PlantResponse object representing the retrieved plant.</returns>
    public async override Task<PlantResponse> Get(GetPlantRequest request, ServerCallContext context)
    {
        try
        {
            var plant = await repository.GetPlantAsync(request.Username, request.PlantMAC);
            return MapToPlantResponse(plant, request.Number);
        }
        catch (InvalidOperationException ex)
        {
            // Plant not found or other repository errors
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
    }

    /// <summary>
    /// Retrieves a list of plants associated with the specified username.
    /// </summary>
    /// <param name="request">The request containing the username to filter plants.</param>
    /// <param name="context">The server call context.</param>
    /// <returns>A response containing a list of plant responses.</returns>
    public async override Task<GetManyPlantResponse> GetPlantsByUsername(GetPlantsByUsernameRequest request,
        ServerCallContext context)
    {
        var plants = repository.GetMany(request.Username);
        var response = new GetManyPlantResponse();
        foreach (var plant in await plants.ToListAsync())
        {
            response.Plants.Add(MapToPlantResponse(plant, request.Number));
        }
        return response;
    }

    /// <summary>
    /// Updates a Plant entity with new configuration details.
    /// </summary>
    /// <param name="request">The request containing the updated plant information.</param>
    /// <param name="context">The context of the server call.</param>
    /// <returns>An empty response indicating successful update.</returns>
    public async override Task<Empty> Update(UpdatePlantRequest request, ServerCallContext context)
    {
        var plant = await repository.GetPlantAsync(request.Username, request.PlantMAC);
    
        if (plant == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Plant not found"));
        }
    
        plant.Name = request.Name;
        plant.OptimalTemperature = request.OptimalTemperature;
        plant.OptimalAirHumidity = request.OptimalAirHumidity;
        plant.OptimalSoilHumidity = request.OptimalSoilHumidity;
        plant.OptimalLightIntensity = request.OptimalLightIntensity;
        plant.Scale = (Entities.plant.TemperatureScale)(int)request.TemperatureScale;
    
        await repository.UpdateAsync(plant);
        return new Empty();
    }

    /// <summary>
    /// Maps a Plant entity to an optimal configuration PlantResponse.
    /// </summary>
    /// <param name="entity">The Plant entity to map.</param>
    /// <returns>A PlantResponse object representing the optimal configuration of the plant.</returns>
    private PlantResponse MapToOptimalConfiguration(Plant entity)
    {
        return new PlantResponse
        {
            OptimalTemperature = entity.OptimalTemperature,
            OptimalAirHumidity = entity.OptimalAirHumidity,
            OptimalSoilHumidity = entity.OptimalSoilHumidity,
            OptimalLightIntensity = entity.OptimalLightIntensity,
            OptimalLightPeriodSeconds = entity.OptimalLightPeriod,
            TemperatureScale = (TemperatureScale)entity.Scale
        };
    }

    /// <summary>
    /// Maps a Plant entity to a PlantResponse object.
    /// </summary>
    /// <param name="entity">The Plant entity to map.</param>
    /// <returns>A PlantResponse object representing the mapped plant data.</returns>
    private PlantResponse MapToPlantResponse(Plant entity, int? number)
    {
        var response = MapToOptimalConfiguration(entity);
        response.PlantMAC = entity.MAC;
        response.Name = entity.Name;
        response.TemperatureScale = (TemperatureScale)entity.Scale;

        response.CurrentTemperature = new TemperatureResponse
        {
            Value = entity.Temperatures.LastOrDefault()?.Value ?? 0,
            PreviousValuesList = {entity.Temperatures.OrderByDescending(temperature => temperature.Id).Take(number ?? int.MaxValue).Select(temperature => temperature.Value ?? 0)}
        };
        
        response.CurrentAirHumidity = new AirHumidityResponse
        {
            Value = entity.AirHumidities.LastOrDefault()?.Value ?? 0,
            PreviousValuesList = {entity.AirHumidities.OrderByDescending(humidity => humidity.Id).Take(number ?? int.MaxValue).Select(humidity => humidity.Value ?? 0)}
        };
        
        response.CurrentSoilHumidity = new SoilHumidityResponse
        {
            Value = entity.SoilHumidities.LastOrDefault()?.Value ?? 0,
            PreviousValuesList = {entity.SoilHumidities.OrderByDescending(soilHumidity => soilHumidity.Id).Take(number ?? int.MaxValue).Select(soilHumidity => soilHumidity.Value ?? 0)}
        };
        
        response.CurrentLightIntensity = new LightIntensityResponse
        {
            Value = entity.LightIntensities.LastOrDefault()?.Value ?? 0,
            PreviousValuesList = {entity.LightIntensities.OrderByDescending(light => light.Id).Take(number ?? int.MaxValue).Select(light => light.Value ?? 0)}
        };
        
        return response;
    }
}