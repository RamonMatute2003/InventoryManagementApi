using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleDto> CreateRoleAsync(RoleDto roleDto)
    {
        if(await _roleRepository.RoleExistsAsync(roleDto.RoleName))
        {
            throw new InvalidOperationException("El rol ya existe.");
        }

        return await _roleRepository.CreateRoleAsync(roleDto);
    }
}