using Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace GrpcService.Services;

public class UserService : UserServiceProto.UserServiceProtoBase
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public override async Task<UserResponse> Create(CreateUserRequest request, ServerCallContext context)
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
            var created = await _repository.CreateAsync(user);
            return await Task.FromResult(MapToResponse(created));
        }
        catch (InvalidOperationException ex)
        {
            // User already exists
            throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
        }
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
            // User not found
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

            return await Task.FromResult(MapToResponse(user));
        }
        catch (InvalidOperationException ex)
        {
            // User not found
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
            // User not found
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
    }

    public override async Task<GetManyUserResponse> GetAll(Empty request, ServerCallContext context)
    {
        try
        {
            var users = await _repository.GetMany().ToListAsync();
            
            var response = new GetManyUserResponse();
            foreach (var user in users)
            {
                response.Users.Add(MapToResponse(user));
            }
            
            return response;
        }
        catch (Exception ex)
        {
            // Handles database failure
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }
    
    private UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Name = user.Name,
            Username = user.Username,
            Email = user.Email
            //Do not send the password to the business layer
        };
    }
}



