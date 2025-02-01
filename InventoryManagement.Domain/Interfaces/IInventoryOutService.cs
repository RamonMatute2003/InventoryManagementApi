using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Interfaces;

public interface IInventoryOutService
{
    Task<IEnumerable<InventoryOutDto>> GetAllAsync();
    Task<InventoryOutDto?> GetByIdAsync(int id);
    Task<InventoryOutDto> CreateInventoryOutAsync(CreateInventoryOutDto inventoryOutDto, int userId);
    Task<IEnumerable<InventoryOutDto>> GetFilteredInventoryOutsAsync(DateTime? startDate, DateTime? endDate, int? branchId, string? status);
    Task<bool> ReceiveInventoryOutAsync(int id, int receivedByUserId);
    Task<ProductDetailsDto?> GetProductDetailsAsync(int productId);
}