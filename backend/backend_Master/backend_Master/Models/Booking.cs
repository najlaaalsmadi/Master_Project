using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? StudentId { get; set; }

    public int? EventId { get; set; }

    public DateOnly? BookingDate { get; set; }

    public string? Status { get; set; }

    public virtual Event? Event { get; set; }

    public virtual User? Student { get; set; }
}
