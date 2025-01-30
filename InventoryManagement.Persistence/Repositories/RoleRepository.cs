using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Persistence.Contexts;
using InventoryManagement.Persistence.Models;
using InventoryManagement.Shared.Models;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Persistence.Repositories;
public class RoleRepository : IRoleRepository
{
    private readonly InventoryDbContext _context;

    public RoleRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await _context.Roles.AnyAsync(r => r.RoleName == roleName);
    }

    public async Task<bool> RoleExistsAsync(int roleId)
    {
        return await _context.Roles.AnyAsync(r => r.IdRole == roleId);
    }

    public async Task<RoleDto> CreateRoleAsync(RoleDto roleDto)
    {
        var newRole = new Role
        {
            RoleName = roleDto.RoleName
        };

        _context.Roles.Add(newRole);
        await _context.SaveChangesAsync();

        return new RoleDto
        {
            IdRole = newRole.IdRole,
            RoleName = newRole.RoleName
        };
    }
}