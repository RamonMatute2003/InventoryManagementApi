using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Services;

public class InventoryOutService : IInventoryOutService
{
    private readonly IInventoryOutRepository _inventoryOutRepository;

    public InventoryOutService(IInventoryOutRepository inventoryOutRepository)
    {
        _inventoryOutRepository = inventoryOutRepository;
    }

    public async Task<IEnumerable<InventoryOutDto>> GetAllAsync()
    {
        return await _inventoryOutRepository.GetAllAsync();
    }

    public async Task<InventoryOutDto?> GetByIdAsync(int id)
    {
        return await _inventoryOutRepository.GetByIdAsync(id);
    }

    public async Task<InventoryOutDto> CreateInventoryOutAsync(CreateInventoryOutDto inventoryOutDto, int userId)
    {
        return await _inventoryOutRepository.CreateAsync(inventoryOutDto, userId);
    }

    public async Task<IEnumerable<InventoryOutDto>> GetFilteredInventoryOutsAsync(DateTime? startDate, DateTime? endDate, int? branchId, string? status)
    {
        return await _inventoryOutRepository.GetFilteredInventoryOutsAsync(startDate, endDate, branchId, status);
    }

    public async Task<bool> ReceiveInventoryOutAsync(int id, int receivedByUserId)
    {
        return await _inventoryOutRepository.ReceiveInventoryOutAsync(id, receivedByUserId);
    }

    public async Task<ProductDetailsDto?> GetProductDetailsAsync(int productId)
    {
        return await _inventoryOutRepository.GetProductDetailsAsync(productId);
    }
}