using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Interfaces;

public interface IInventoryOutRepository
{
    Task<IEnumerable<InventoryOutDto>> GetAllAsync();
    Task<InventoryOutDto?> GetByIdAsync(int id);
    Task<InventoryOutDto> CreateAsync(CreateInventoryOutDto inventoryOut, int userId);
    Task<IEnumerable<FilteredInventoryOutDto>> GetFilteredInventoryOutsAsync(DateTime? startDate, DateTime? endDate, int? branchId, string? status);
    Task<bool> ReceiveInventoryOutAsync(int id, int receivedByUserId);
    Task<ProductDetailsDto?> GetProductDetailsAsync(int productId);
}