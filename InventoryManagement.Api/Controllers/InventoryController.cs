using InventoryManagement.Api.Helpers;
using InventoryManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[ApiController]
[Route("api/inventory")]
[Authorize]
public class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    [HttpGet("{productId}/available-stock")]
    public async Task<IActionResult> GetAvailableStock(int productId)
    {
        var availableStock = await inventoryService.GetAvailableStockAsync(productId);

        return availableStock >= 0
            ? ApiResponseHelper.Ok(new { ProductId = productId, AvailableStock = availableStock }, "Stock disponible obtenido correctamente.")
            : ApiResponseHelper.NotFound($"No se encontró el producto con ID {productId} en el inventario.");
    }
}