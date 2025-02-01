namespace InventoryManagement.Shared.Models;

public class ProductDetailsDto
{
    public int IdProduct { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public List<ProductLotDto> Lots { get; set; } = new();
}