using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateOnly? Date { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<Newsletter> Newsletters { get; set; } = new List<Newsletter>();
}
