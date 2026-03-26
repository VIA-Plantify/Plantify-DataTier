using Entities;
using RepositoryContracts;

namespace EFC.Repositories;

public class UserRepository : IUserRepository
{
    private List<User> users;

    public async Task<User> CreateAsync(User user)
    {
        users.Add(user);
        return await Task.FromResult(user);
    }
    public async Task<User> GetByEmailAsync(string email)
    {
        var user = users.FirstOrDefault(u => u.Email == email);
        if (user == null)
            throw new InvalidOperationException($"User with email {user?.Email} not found.");
        return await Task.FromResult(user);
    }
    public async Task<User> GetByUsernameAsync(string username)
    {
        var user = users.FirstOrDefault(u => u.Username == username);
        if (user == null)
            throw new InvalidOperationException($"User with username {user?.Username} not found.");
        return await Task.FromResult(user);
    }
    public async Task DeleteAsync(string username)
    {
        var user = users.FirstOrDefault(u => u.Username == username);
        if (user != null)
        {
            users.Remove(user);
        }
        await Task.CompletedTask;
    }
    public async Task UpdateAsync(User user)
    {
        var existingUser = users.FirstOrDefault(u => u.Username == user.Username);
        if (existingUser != null)
        {
            var index = users.IndexOf(existingUser);
            users[index] = user;
        }
        await Task.CompletedTask;
    }
    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}