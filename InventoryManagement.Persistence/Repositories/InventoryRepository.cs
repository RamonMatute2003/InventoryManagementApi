using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Persistence.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _context;

    public InventoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetAvailableStockAsync(int productId)
    {
        return await _context.InventoryLots
            .Where(l => l.IdProduct == productId)
            .SumAsync(l => l.BatchQuantity);
    }
}