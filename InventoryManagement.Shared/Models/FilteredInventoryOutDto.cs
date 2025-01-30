namespace InventoryManagement.Shared.Models;

public class FilteredInventoryOutDto : InventoryOutDto
{
    public string? UserName { get; set; }
    public string? BranchName { get; set; }
    public string Status { get; set; } = null!;
    public int TotalUnits { get; set; }
    public string? ReceivedBy { get; set; }
    public DateTime? ReceivedDate { get; set; }
}