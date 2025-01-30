namespace InventoryManagement.Shared.Models;

public class CreateInventoryOutDto
{
    public decimal TotalCost { get; set; }
    public int IdBranch { get; set; }
    public int IdUser { get; set; }
    public List<CreateInventoryOutDetailDto> Details { get; set; } = new();
}