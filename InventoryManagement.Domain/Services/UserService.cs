using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
    {
        var userExists = await _userRepository.UserExistsAsync(userDto.UserName);
        if(userExists)
        {
            throw new InvalidOperationException($"⚠️ El usuario '{userDto.UserName}' ya existe.");
        }

        var roleExists = await _roleRepository.RoleExistsAsync(userDto.IdRole);
        if(!roleExists)
        {
            throw new InvalidOperationException($"❌ El rol con Id {userDto.IdRole} no existe.");
        }

        return await _userRepository.CreateUserAsync(userDto);
    }
}