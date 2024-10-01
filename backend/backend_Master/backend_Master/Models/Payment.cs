using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? UserId { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PayerId { get; set; }

    public string? Status { get; set; }

    public virtual User? User { get; set; }
}
