using System;
using System.Collections.Generic;

namespace InventoryManagement.Persistence.Models;

public partial class InventoryLot
{
    public int IdBatch { get; set; }

    public int BatchQuantity { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public decimal Cost { get; set; }

    public int IdProduct { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual ICollection<InventoryOutDetail> InventoryOutDetails { get; set; } = new List<InventoryOutDetail>();
}
