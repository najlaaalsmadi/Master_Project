using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class UserCourse
{
    public int EnrollmentId { get; set; }

    public int? UserId { get; set; }

    public int? CourseId { get; set; }

    public DateTime? EnrollmentDate { get; set; }

    public int? Progress { get; set; }

    public bool? Completed { get; set; }

    public virtual Course? Course { get; set; }

    public virtual User? User { get; set; }
}
