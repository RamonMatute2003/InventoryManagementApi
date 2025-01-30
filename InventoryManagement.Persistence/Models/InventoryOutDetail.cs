using System;
using System.Collections.Generic;

namespace InventoryManagement.Persistence.Models;

public partial class InventoryOutDetail
{
    public int IdOutDetail { get; set; }

    public int Quantity { get; set; }

    public decimal Cost { get; set; }

    public int IdOutHeader { get; set; }

    public int IdProduct { get; set; }

    public int IdBatch { get; set; }

    public virtual InventoryLot IdBatchNavigation { get; set; } = null!;

    public virtual InventoryOutHeader IdOutHeaderNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
