using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? TrainerId { get; set; }

    public int? CategoryId { get; set; }

    public decimal? Price { get; set; }

    public int? Duration { get; set; }

    public int? AllowedStudents { get; set; }

    public string? Syllabus { get; set; }

    public string? Tools { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? Rating { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<LearningEquipment> LearningEquipments { get; set; } = new List<LearningEquipment>();

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual Trainer? Trainer { get; set; }
}
