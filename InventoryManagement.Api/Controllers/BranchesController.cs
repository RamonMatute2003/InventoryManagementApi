using InventoryManagement.Api.Helpers;
using InventoryManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[Route("api/branches")]
[ApiController]
[Authorize]
public class BranchesController(IBranchService branchService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBranches()
    {
        var branches = await branchService.GetAllBranchesAsync();

        return branches != null && branches.Count > 0
            ? ApiResponseHelper.Ok(branches, "Sucursales obtenidas correctamente.")
            : ApiResponseHelper.NotFound("No se encontraron sucursales registradas.");
    }
}