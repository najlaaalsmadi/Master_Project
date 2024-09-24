using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public int? UserId { get; set; }

    public string? AccessLevel { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
