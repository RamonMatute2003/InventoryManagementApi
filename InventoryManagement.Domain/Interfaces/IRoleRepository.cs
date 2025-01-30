using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Interfaces;

public interface IRoleRepository
{
    Task<bool> RoleExistsAsync(string roleName);
    Task<RoleDto> CreateRoleAsync(RoleDto role);
    Task<bool> RoleExistsAsync(int roleId);
}