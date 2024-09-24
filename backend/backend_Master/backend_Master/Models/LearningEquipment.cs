using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class LearningEquipment
{
    public int EquipmentId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? Stock { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CourseId { get; set; }

    public virtual Course? Course { get; set; }
}
