using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Interfaces;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(CreateUserDto userDto);
}