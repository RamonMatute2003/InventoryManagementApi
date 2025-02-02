using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Persistence.Contexts;
using InventoryManagement.Persistence.Models;
using InventoryManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Persistence.Repositories;

public class RoleRepository(InventoryDbContext context) : IRoleRepository
{
    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await context.Roles.AnyAsync(r => r.RoleName == roleName);
    }

    public async Task<bool> RoleExistsAsync(int roleId)
    {
        return await context.Roles.AnyAsync(r => r.IdRole == roleId);
    }

    public async Task<RoleDto> CreateRoleAsync(RoleDto roleDto)
    {
        var newRole = new Role
        {
            RoleName = roleDto.RoleName
        };

        context.Roles.Add(newRole);
        await context.SaveChangesAsync();

        return new RoleDto
        {
            IdRole = newRole.IdRole,
            RoleName = newRole.RoleName
        };
    }
}