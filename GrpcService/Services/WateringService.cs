using Entities.plant;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RepositoryContracts;

namespace GrpcService.Services;

public class WateringService (IWateringRepository repository) : WateringServiceProto.WateringServiceProtoBase
{
    public override async Task<Empty> Create(CreateWateringRequest request, ServerCallContext context)
    {
        var watering = new Watering
        {
            PumpTimeInSeconds = request.PumpTimeInSeconds,
            LastWaterTime = request.LastWaterTime.ToDateTime(),
            WaterLevel = request.WaterLevel,
            PlantMAC = request.PlantMAC,
        };
        
        await repository.CreateWatering(watering);
        return new Empty();
    }

    public override async Task<WateringResponse?> GetLatest(GetLatestWateringDataRequest request, ServerCallContext context)
    {
        return ProtoUtils.MapToWateringResponse(await repository.GetWateringAsync(request.PlantMAC));
    }
}