using Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace GrpcService.Services;

public class UserService : UserServiceProto.UserServiceProtoBase
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _repository;

    public UserService(ILogger<UserService> logger, IUserRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public override async Task<UserResponse> Create(CreateUserRequest request, ServerCallContext context)
    {
        // No need for rpc handling because we are doing it through get ig
        var user = new User
        {
            Name = request.Name,
            Username = request.Username,
            Password = request.Password,
            Email = request.Email
        };

        var created = await _repository.CreateAsync(user);

        return MapToResponse(created);
    }

    public override async Task<Empty> Delete(DeleteUserRequest request, ServerCallContext context)
    {
        try
        { 
            await _repository.DeleteAsync(request.Username);
            return new Empty();
        }
        catch (InvalidOperationException ex)
        {
            // In case we are trying to delete a user that doesn't exist
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
    }

    public override async Task<UserResponse> Get(GetUserRequest request, ServerCallContext context)
    {
        try
        {
            User user;

            if (!string.IsNullOrEmpty(request.Username))
                user = await _repository.GetByUsernameAsync(request.Username);
            else
                user = await _repository.GetByEmailAsync(request.Email);

            return MapToResponse(user);
        }
        catch (InvalidOperationException ex)
        {
            // Rpc exeption that can be hanled on the bussiness tier
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
    }

    public override async Task<Empty> Update(UpdateUserRequest request, ServerCallContext context)
    {
        var user = new User
        {
            Name = request.Name,
            Username = request.Username,
            Password = request.Password,
            Email = request.Email
        };

        try
        {
            await _repository.UpdateAsync(user);
            return new Empty();
        }
        catch (InvalidOperationException ex)
        {
            // User not found or cannot update (but our bussiness tier doesn't have rpc handling for update)
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
    }

    public override async Task<GetManyUserResponse> GetAll(Empty request, ServerCallContext context)
    {
        // No need for rpc handling, "no users" are naturally handled on the business tier
        var users = await _repository.GetMany().ToListAsync();

        var response = new GetManyUserResponse();

        foreach (var user in users)
        {
            response.Users.Add(MapToResponse(user));
        }

        return response;
    }
    
    private UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Name = user.Name,
            Username = user.Username,
            Password = user.Password,
            Email = user.Email
        };
    }
}