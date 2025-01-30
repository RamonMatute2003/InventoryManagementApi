namespace InventoryManagement.Domain.Interfaces;

public interface IInventoryRepository
{
    Task<int> GetAvailableStockAsync(int productId);
}