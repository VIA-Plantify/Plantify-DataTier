using Entities.plant;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RepositoryContracts;

namespace GrpcService.Services;

public class SensorService(ISensorRepository repository) : SensorServiceProto.SensorServiceProtoBase
{
    public async override Task<Empty> Create(CreateSensorDataRequest request, ServerCallContext context)
    {
        var sensor = new SensorData
        {
            SoilHumidity = request.SoilHumidity,
            Temperature = request.Temperature,
            LightIntensity = request.LightIntensity,
            AirHumidity = request.AirHumidity,
            PlantMAC = request.PlantMAC,
            Timestamp = request.Timestamp.ToDateTime()
        };
        await repository.CreateSensorData(sensor);
    return new Empty();
    }

    public async override Task<SensorResponse?> GetLatest(GetLatestSensorDataRequest request, ServerCallContext context)
    {
        return ProtoUtils.MapToSensorResponse(await repository.GetLatestAsync(request.PlantMac));
    }
}