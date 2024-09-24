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

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual User? User { get; set; }
}
