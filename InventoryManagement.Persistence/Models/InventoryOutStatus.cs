using System;
using System.Collections.Generic;

namespace InventoryManagement.Persistence.Models;

public partial class InventoryOutStatus
{
    public int IdStatus { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<InventoryOutHeader> InventoryOutHeaders { get; set; } = new List<InventoryOutHeader>();
}
