using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Interfaces;

public interface IProductRepository
{
    Task<List<ProductDto>> GetAllProductsAsync();
}