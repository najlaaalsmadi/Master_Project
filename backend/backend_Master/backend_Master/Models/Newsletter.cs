using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Newsletter
{
    public int NewsletterId { get; set; }

    public string? Email { get; set; }

    public DateTime? SubscribedAt { get; set; }

    public int? EventId { get; set; }

    public virtual Event? Event { get; set; }
}
