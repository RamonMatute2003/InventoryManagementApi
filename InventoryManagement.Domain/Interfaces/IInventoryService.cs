namespace InventoryManagement.Domain.Interfaces;

public interface IInventoryService
{
    Task<int> GetAvailableStockAsync(int productId);
}