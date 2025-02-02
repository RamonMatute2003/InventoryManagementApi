using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Persistence.Repositories;

public class InventoryRepository(InventoryDbContext context) : IInventoryRepository
{
    public async Task<int> GetAvailableStockAsync(int productId)
    {
        return await context.InventoryLots
            .Where(l => l.IdProduct == productId)
            .SumAsync(l => l.BatchQuantity);
    }
}