using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Services;

public class UserService(IUserRepository userRepository, IRoleRepository roleRepository) : IUserService
{
    public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
    {
        var userExists = await userRepository.UserExistsAsync(userDto.UserName);
        if(userExists)
        {
            throw new InvalidOperationException($"El usuario '{userDto.UserName}' ya existe.");
        }

        var roleExists = await roleRepository.RoleExistsAsync(userDto.IdRole);
        if(!roleExists)
        {
            throw new InvalidOperationException($"El rol con Id {userDto.IdRole} no existe.");
        }

        return await userRepository.CreateUserAsync(userDto);
    }
}