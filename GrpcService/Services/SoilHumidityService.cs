using Entities.plant;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RepositoryContracts;

namespace GrpcService.Services;

/// <summary>
/// gRPC service for managing soil humidity data.
/// Provides functionality to create soil humidity records associated with a plant identified by its MAC address.
/// </summary>
public class SoilHumidityService(ISoilHumidityRepository repository) : SoilHumidityServiceProto.SoilHumidityServiceProtoBase
{
    /// <summary>
    /// Creates a new soil humidity record for a given plant.
    /// </summary>
    /// <param name="request">The request containing the MAC address of the plant and the soil humidity value.</param>
    /// <param name="context">The context of the server call.</param>
    /// <returns>An empty response indicating successful creation.</returns>
    public override async Task<Empty> Create(CreateSoilHumidityRequest request, ServerCallContext context)
    {
        await repository.CreateAsync(request.PlantMAC, new SoilHumidity { Value = request.Value });
        return new Empty();
    }
}