namespace InventoryManagement.Shared.Models;

public class BaseUserDto
{
    public int IdUser { get; set; }
    public string UserName { get; set; } = null!;
    public string Role { get; set; } = null!;
}