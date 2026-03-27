using EFC.DataAccess;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EFC.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PlantifyContext context;

    public UserRepository(PlantifyContext context)
    {
        this.context = context;
        context.Database.EnsureCreated();
    }

    public async Task<User> CreateAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }
    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new InvalidOperationException($"User with email '{email}' not found.");
        }
        return user;
    }
    public async Task<User> GetByUsernameAsync(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            throw new InvalidOperationException($"User with username '{username}' not found.");
        }
        return user;
    }
    public async Task DeleteAsync(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }
    public async Task UpdateAsync(User user)
    {
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"User with username '{user.Username}' not found.");
        }
        existingUser.Email = user.Email;
        existingUser.Name = user.Name;
        context.Users.Update(existingUser);
        await context.SaveChangesAsync();
    }
    public IQueryable<User> GetMany()
    {
        return context.Users.AsQueryable();
    }
}