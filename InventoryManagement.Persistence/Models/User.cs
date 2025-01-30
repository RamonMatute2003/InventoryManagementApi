using System;
using System.Collections.Generic;

namespace InventoryManagement.Persistence.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int IdRole { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<InventoryOutHeader> InventoryOutHeaderIdUserNavigations { get; set; } = new List<InventoryOutHeader>();

    public virtual ICollection<InventoryOutHeader> InventoryOutHeaderReceivedByNavigations { get; set; } = new List<InventoryOutHeader>();
}
