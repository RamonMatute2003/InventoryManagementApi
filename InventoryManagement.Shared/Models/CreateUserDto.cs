namespace InventoryManagement.Shared.Models;

public class CreateUserDto
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int IdRole { get; set; }
}