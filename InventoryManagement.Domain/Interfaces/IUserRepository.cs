using InventoryManagement.Shared.Models;

namespace InventoryManagement.Domain.Interfaces;

public interface IUserRepository
{
    Task<UserWithPasswordDto?> GetUserByUsernameAsync(string username);
    Task<UserDto> CreateUserAsync(CreateUserDto user);
    Task<bool> UserExistsAsync(string username);
}