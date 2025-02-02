using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Persistence.Contexts;
using InventoryManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Persistence.Repositories;

public class ProductRepository(InventoryDbContext context) : IProductRepository
{
    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        return await context.Products
            .Select(p => new ProductDto
            {
                Id = p.IdProduct,
                Name = p.Name,
                Cost = p.Cost
            })
            .ToListAsync();
    }
}