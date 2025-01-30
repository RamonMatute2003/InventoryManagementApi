namespace InventoryManagement.Shared.Models;

public class UserWithPasswordDto : BaseUserDto
{
    public string PasswordHash { get; set; } = null!;
}