using System;
using System.Collections.Generic;

namespace InventoryManagement.Persistence.Models;

public partial class Branch
{
    public int IdBranch { get; set; }

    public string BranchName { get; set; } = null!;

    public string? BranchLocation { get; set; }

    public virtual ICollection<InventoryOutHeader> InventoryOutHeaders { get; set; } = new List<InventoryOutHeader>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
