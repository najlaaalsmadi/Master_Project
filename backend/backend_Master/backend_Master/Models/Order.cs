using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
