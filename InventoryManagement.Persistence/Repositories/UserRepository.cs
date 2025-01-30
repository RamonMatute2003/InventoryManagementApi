using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Persistence.Contexts;
using InventoryManagement.Persistence.Models;
using InventoryManagement.Shared.Models;

using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly InventoryDbContext _context;

    public UserRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<UserWithPasswordDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _context.Users
            .Include(u => u.IdRoleNavigation)
            .Where(u => u.UserName == username)
            .Select(u => new UserWithPasswordDto
            {
                IdUser = u.IdUser,
                UserName = u.UserName,
                PasswordHash = u.PasswordHash,
                Role = u.IdRoleNavigation.RoleName
            })
            .FirstOrDefaultAsync();

        return user;
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.UserName == username);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
    {
        var newUser = new User
        {
            UserName = userDto.UserName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            IdRole = userDto.IdRole
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        var role = await _context.Roles
            .Where(r => r.IdRole == newUser.IdRole)
            .Select(r => r.RoleName)
            .FirstOrDefaultAsync();

        return new UserDto
        {
            IdUser = newUser.IdUser,
            UserName = newUser.UserName,
            Role = role ?? "Desconocido"
        };
    }
}