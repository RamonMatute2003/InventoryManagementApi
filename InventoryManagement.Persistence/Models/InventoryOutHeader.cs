using System;
using System.Collections.Generic;

namespace InventoryManagement.Persistence.Models;

public partial class InventoryOutHeader
{
    public int IdOutHeader { get; set; }

    public DateTime OutDate { get; set; }

    public decimal TotalCost { get; set; }

    public int IdUser { get; set; }

    public int IdBranch { get; set; }

    public int IdStatus { get; set; }

    public int? ReceivedBy { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public virtual Branch IdBranchNavigation { get; set; } = null!;

    public virtual InventoryOutStatus IdStatusNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<InventoryOutDetail> InventoryOutDetails { get; set; } = new List<InventoryOutDetail>();

    public virtual User? ReceivedByNavigation { get; set; }
}
