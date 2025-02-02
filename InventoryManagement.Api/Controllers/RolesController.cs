using InventoryManagement.Api.DTOs;
using InventoryManagement.Api.Helpers;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
    {
        try
        {
            var roleDto = new RoleDto
            {
                RoleName = createRoleDto.RoleName
            };

            var createdRole = await _roleService.CreateRoleAsync(roleDto);
            return ApiResponseHelper.Created(createdRole, "Rol creado exitosamente.");
        } catch(InvalidOperationException ex)
        {
            return ApiResponseHelper.Conflict(ex.Message);
        }
    }
}