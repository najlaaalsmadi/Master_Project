using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string? EventTitle { get; set; }

    public DateOnly? EventDate { get; set; }

    public TimeOnly? EventTime { get; set; }

    public string? Location { get; set; }

    public int? Participants { get; set; }

    public string? Speaker { get; set; }

    public string? Summary { get; set; }

    public string? Learnings { get; set; }

    public string? Features { get; set; }

    public int? SeatsAvailable { get; set; }

    public int? Topics { get; set; }

    public int? Exams { get; set; }

    public int? Articles { get; set; }

    public int? Certificates { get; set; }

    public string? ImagePath { get; set; }

    public string? MapUrl { get; set; }

    public string? ZoomLink { get; set; }

    public string? ZoomPassword { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Newsletter> Newsletters { get; set; } = new List<Newsletter>();
}
