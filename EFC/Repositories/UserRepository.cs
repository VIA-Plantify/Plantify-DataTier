using Entities;
using RepositoryContracts;

namespace EFC.Repositories;

public class UserRepository : IUserRepository
{

    public Task<User> CreateAsync(User user)
    {
        throw new NotImplementedException();
    }
    public Task<User> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }
    public Task<User> GetByUsernameAsync(string username)
    {
        throw new NotImplementedException();
    }
    public Task DeleteAsync(string username)
    {
        throw new NotImplementedException();
    }
    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }
    public Task<IEnumerable<User>> GetManyAsync()
    {
        throw new NotImplementedException();
    }
}