namespace InventoryManagement.Shared.Models;

public class ProductLotDto
{
    public int BatchId { get; set; }
    public int BatchQuantity { get; set; }
    public decimal Cost { get; set; }
    public DateOnly ExpirationDate { get; set; }
}