using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RepositoryContracts;

namespace GrpcService.Services;

public class WateringService (IWateringRepository repository) : WateringServiceProto.WateringServiceProtoBase
{
    public override Task<Empty> Create(CreateWateringRequest request, ServerCallContext context)
    {
        //TODO
        return base.Create(request, context);
    }

    public override Task<WateringResponse> GetLatest(GetLatestWateringDataRequest request, ServerCallContext context)
    {
        //TODO
        return base.GetLatest(request, context);
    }
}