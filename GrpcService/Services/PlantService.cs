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
            MAC = request.MAC,

            OptimalTemperature = request.OptimalTemperature,
            OptimalAirHumidity = request.OptimalAirHumidity,
            OptimalSoilHumidity = request.OptimalSoilHumidity,
            OptimalLightIntensity = request.OptimalLightIntensity,
            Scale = (Entities.plant.TemperatureScale)(int)request.TemperatureScale
        };

        try
        {
            var createdPlant = await repository.CreateAsync(plant);
            return ProtoUtils.MapToPlantResponse(createdPlant);
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
            var plant = await repository.GetPlantAsync(request.Username, request.PlantMAC,
                request.NumberOfSensorReadings, request.NumberOfWateringReadings);
            return ProtoUtils.MapToPlantResponse(plant);
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
        var plants = repository.GetMany(request.Username, request.NumberOfReadings, request.NumberOfWateringReadings);
        var response = new GetManyPlantResponse();
        foreach (var plant in await plants.ToListAsync())
        {
            response.Plants.Add(ProtoUtils.MapToPlantResponse(plant));
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
        var plant = await repository.GetPlantAsync(request.Username, request.PlantMAC, numberOfSensorReadings: null,
            numberOfWateringReadings: null);

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

        plant.ShouldPredictOptimal = request.ShouldPredictOptimal;

        await repository.UpdateAsync(plant);
        return new Empty();
    }

    

    public override async Task<GetManyPlantResponse> GetAllPlants(Empty request ,ServerCallContext context)
    {
        try
        {
            var plants = await repository.GetAllPlants().ToListAsync();
            
            var response = new GetManyPlantResponse();

            foreach (var plant in plants)
            {
                response.Plants.Add(ProtoUtils.MapToPlantResponse(plant));
            }
            
            return response;
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(StatusCode.Internal, e.Message));
        }
    }
}