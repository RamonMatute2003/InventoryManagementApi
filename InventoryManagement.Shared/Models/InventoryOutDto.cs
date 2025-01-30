namespace InventoryManagement.Shared.Models;

public class InventoryOutDto
{
    public int IdOutHeader { get; set; }
    public DateTime OutDate { get; set; }
    public decimal TotalCost { get; set; }
    public int IdUser { get; set; }
    public int IdBranch { get; set; }
    public int IdStatus { get; set; }
}