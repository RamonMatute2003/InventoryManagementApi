using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Services;

public class RoleService(IRoleRepository roleRepository) : IRoleService
{
    public async Task<RoleDto> CreateRoleAsync(RoleDto roleDto)
    {
        if(await roleRepository.RoleExistsAsync(roleDto.RoleName))
        {
            throw new InvalidOperationException("El rol ya existe.");
        }

        return await roleRepository.CreateRoleAsync(roleDto);
    }
}