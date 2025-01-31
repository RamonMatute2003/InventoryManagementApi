using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Services;
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllProductsAsync();
    }
}