using EFC.DataAccess;
using Entities.plant;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EFC.Repositories;

public class WateringRepository(PlantifyContext context) : IWateringRepository
{
    public async Task CreateWatering(Watering watering)
    {
        await context.Waterings.AddAsync(watering);
        await context.SaveChangesAsync();
        await Task.CompletedTask;
    }

    public async Task<Watering?> GetWateringAsync(string plantMac)
    {
        return await context.Waterings.OrderByDescending(w => w.Id).FirstOrDefaultAsync(w => w.PlantMAC == plantMac);
    }
}