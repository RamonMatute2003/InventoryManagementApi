using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Services;

public class InventoryOutService(IInventoryOutRepository inventoryOutRepository) : IInventoryOutService
{
    public async Task<IEnumerable<InventoryOutDto>> GetAllAsync()
    {
        return await inventoryOutRepository.GetAllAsync();
    }

    public async Task<InventoryOutDto?> GetByIdAsync(int id)
    {
        return await inventoryOutRepository.GetByIdAsync(id);
    }

    public async Task<InventoryOutDto> CreateInventoryOutAsync(CreateInventoryOutDto inventoryOutDto, int userId)
    {
        return await inventoryOutRepository.CreateAsync(inventoryOutDto, userId);
    }

    public async Task<IEnumerable<InventoryOutDto>> GetFilteredInventoryOutsAsync(DateTime? startDate, DateTime? endDate, int? branchId, string? status)
    {
        return await inventoryOutRepository.GetFilteredInventoryOutsAsync(startDate, endDate, branchId, status);
    }

    public async Task<bool> ReceiveInventoryOutAsync(int id, int receivedByUserId)
    {
        return await inventoryOutRepository.ReceiveInventoryOutAsync(id, receivedByUserId);
    }

    public async Task<ProductDetailsDto?> GetProductDetailsAsync(int productId)
    {
        return await inventoryOutRepository.GetProductDetailsAsync(productId);
    }
}