using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Trainer
{
    public int TrainerId { get; set; }

    public int? UserId { get; set; }

    public string? Bio { get; set; }

    public string? Experience { get; set; }

    public string? Specialization { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? PasswordSalt { get; set; }

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Phone { get; set; }

    public string? JobTitle { get; set; }

    public string? ImageProfile { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual User? User { get; set; }

    public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
}
