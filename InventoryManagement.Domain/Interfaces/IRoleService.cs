using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Interfaces;

public interface IRoleService
{
    Task<RoleDto> CreateRoleAsync(RoleDto roleDto);
}