using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Domain.Interfaces;

namespace InventoryManagement.Domain.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<int> GetAvailableStockAsync(int productId)
    {
        return await _inventoryRepository.GetAvailableStockAsync(productId);
    }
}