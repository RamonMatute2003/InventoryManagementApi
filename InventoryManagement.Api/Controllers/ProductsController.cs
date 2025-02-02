using InventoryManagement.Api.Helpers;
using InventoryManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await productService.GetAllProductsAsync();

        return products != null && products.Count > 0
            ? ApiResponseHelper.Ok(products, "Productos obtenidos correctamente.")
            : ApiResponseHelper.NotFound("No se encontraron productos disponibles.");
    }
}