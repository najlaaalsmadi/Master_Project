using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public decimal? Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public int? UserId { get; set; }

    public int? OrderId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ZipCode { get; set; }

    public string? Country { get; set; }

    public string? CountryCallingCode { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Order? Order { get; set; }

    public virtual User? User { get; set; }
}
