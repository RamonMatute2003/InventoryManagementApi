using System;
using System.Collections.Generic;

namespace InventoryManagement.Persistence.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public string ProductCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Cost { get; set; }

    public int? IdBranch { get; set; }

    public virtual Branch? IdBranchNavigation { get; set; }

    public virtual ICollection<InventoryLot> InventoryLots { get; set; } = new List<InventoryLot>();

    public virtual ICollection<InventoryOutDetail> InventoryOutDetails { get; set; } = new List<InventoryOutDetail>();
}
