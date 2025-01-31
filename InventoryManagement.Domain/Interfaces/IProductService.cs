using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
}