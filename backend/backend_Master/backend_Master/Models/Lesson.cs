using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Lesson
{
    public int LessonId { get; set; }

    public int? CourseId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? VideoUrl { get; set; }

    public int? Order { get; set; }

    public virtual Course? Course { get; set; }
}
