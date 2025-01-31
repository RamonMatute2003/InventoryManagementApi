using System.Security.Claims;

using InventoryManagement.Api.Helpers;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[ApiController]
[Route("api/inventory-outs")]
[Authorize]
public class InventoryOutController : ControllerBase
{
    private readonly IInventoryOutService _inventoryOutService;

    public InventoryOutController(IInventoryOutService inventoryOutService)
    {
        _inventoryOutService = inventoryOutService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllInventoryOuts()
    {
        var result = await _inventoryOutService.GetAllAsync();
        return ApiResponseHelper.Ok(result, "✅ Listado de salidas de inventario obtenido correctamente.");
    }

    [HttpGet("detail/{id:int}")]
    public async Task<IActionResult> GetInventoryOutById(int id)
    {
        var result = await _inventoryOutService.GetByIdAsync(id);
        return result == null
            ? ApiResponseHelper.NotFound("❌ Salida de inventario no encontrada.")
            : ApiResponseHelper.Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateInventoryOut([FromBody] CreateInventoryOutDto inventoryOutDto)
    {
        try
        {
            var userClaims = User.FindAll("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").ToList();

            var userIdClaim = userClaims.FirstOrDefault(c => int.TryParse(c.Value, out _));

            var roleClaim = userClaims.FirstOrDefault(c => !int.TryParse(c.Value, out _));

            if(userIdClaim == null || roleClaim == null)
            {
                return ApiResponseHelper.Unauthorized("No se pudo obtener el ID del usuario o su rol.");
            }

            if(!int.TryParse(userIdClaim.Value, out int userId))
            {
                return ApiResponseHelper.Unauthorized($"El Claim NameIdentifier tiene un valor inválido: {userIdClaim.Value}");
            }

            if(roleClaim.Value != "Jefe de Bodega")
            {
                return ApiResponseHelper.Forbidden("⛔ Solo los usuarios con el rol 'Jefe de Bodega' pueden registrar salidas de inventario.");
            }

            var result = await _inventoryOutService.CreateInventoryOutAsync(inventoryOutDto, userId);
            return ApiResponseHelper.Created(result, "✅ Salida de inventario creada exitosamente.");
        } catch(Exception ex){
            return ApiResponseHelper.InternalServerError(ex.Message, ex.ToString());
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllInventoryOuts(
    [FromQuery] DateTime? startDate,
    [FromQuery] DateTime? endDate,
    [FromQuery] int? branchId,
    [FromQuery] string? status)
    {
        var inventoryOuts = await _inventoryOutService.GetFilteredInventoryOutsAsync(startDate, endDate, branchId, status);
        return ApiResponseHelper.Ok(inventoryOuts, "✅ Listado de salidas de inventario obtenido correctamente.");
    }

    [HttpPut("{id}/receive")]
    public async Task<IActionResult> ReceiveInventoryOut(int id)
    {
        var userClaims = User.FindAll("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").ToList();

        var userIdClaim = userClaims.FirstOrDefault(c => int.TryParse(c.Value, out _));

        if(userIdClaim == null)
        {
            return ApiResponseHelper.Unauthorized("No se encontró el Claim NameIdentifier en el token.");
        }

        if(!int.TryParse(userIdClaim.Value, out int userId))
        {
            return ApiResponseHelper.Unauthorized($"El Claim NameIdentifier tiene un valor inválido: {userIdClaim.Value}");
        }

        var result = await _inventoryOutService.ReceiveInventoryOutAsync(id, userId);

        return result
            ? ApiResponseHelper.Ok("✅ Salida de inventario marcada como recibida.")
            : ApiResponseHelper.NotFound("❌ No se encontró la salida de inventario o ya estaba recibida.");
    }
}