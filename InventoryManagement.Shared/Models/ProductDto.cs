﻿namespace InventoryManagement.Shared.Models;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}