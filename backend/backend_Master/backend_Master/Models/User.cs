using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? PasswordHash { get; set; }

    public string? PasswordSalt { get; set; }

    public string? Otp { get; set; }

    public string? BiographicaldetailsCv { get; set; }

    public string? Phone { get; set; }

    public string? JobTitle { get; set; }

    public string? Imageprofile { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<HandmadeProduct> HandmadeProducts { get; set; } = new List<HandmadeProduct>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PaymentsCourse> PaymentsCourses { get; set; } = new List<PaymentsCourse>();

    public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();

    public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
}
