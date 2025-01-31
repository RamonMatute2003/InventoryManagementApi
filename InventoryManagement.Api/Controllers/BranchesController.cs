using InventoryManagement.Api.Helpers;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Enums;
using InventoryManagement.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[Route("api/branches")]
[ApiController]
[Authorize]
public class BranchesController : ControllerBase
{
    private readonly IBranchService _branchService;

    public BranchesController(IBranchService branchService)
    {
        _branchService = branchService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBranches()
    {
        var branches = await _branchService.GetAllBranchesAsync();

        return branches != null && branches.Count > 0
            ? ApiResponseHelper.Ok(branches, "✅ Sucursales obtenidas correctamente.")
            : ApiResponseHelper.NotFound("❌ No se encontraron sucursales registradas.");
    }
}