using Entities;
using Grpc.Core;
using RepositoryContracts;

namespace GrpcService.Services;

/// <summary>
/// Represents an authentication service that handles user login using gRPC.
/// </summary>
public class AuthService(IUserRepository repository) : AuthServiceProto.AuthServiceProtoBase
{
    /// <summary>
    /// Handles user login by validating the provided credentials and returning user information.
    /// </summary>
    /// <param name="request">The authentication request containing either a username or email.</param>
    /// <param name="context">The server call context for managing RPC calls.</param>
    /// <returns>A UserResponse object containing user details if the login is successful, otherwise throws an exception.</returns>
    public override async Task<UserResponse> Login(AuthRequest request, ServerCallContext context)
    {
        try
        {
            User user;

            if (!string.IsNullOrEmpty(request.Username))
                user = await repository.GetByUsernameAsync(request.Username);
            else
                user = await repository.GetByEmailAsync(request.Email);
            var response = new UserResponse()
            {
                Email = user.Email,
                Password = user.Password, //return the password
                Name = user.Name,
                Username = user.Username
            };
            return await Task.FromResult(response);
        }
        catch (InvalidOperationException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
    }
}