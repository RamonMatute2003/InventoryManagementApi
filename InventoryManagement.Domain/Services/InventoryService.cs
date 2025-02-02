using InventoryManagement.Domain.Interfaces;

namespace InventoryManagement.Domain.Services;

public class InventoryService(IInventoryRepository inventoryRepository) : IInventoryService
{
    public async Task<int> GetAvailableStockAsync(int productId)
    {
        return await inventoryRepository.GetAvailableStockAsync(productId);
    }
}