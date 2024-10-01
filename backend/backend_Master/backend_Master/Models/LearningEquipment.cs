using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class LearningEquipment
{
    public int EquipmentId { get; set; }

    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CourseId { get; set; }

    public string? ImageUrl1 { get; set; }

    public string? ImageUrl2 { get; set; }

    public string? ImageUrl3 { get; set; }

    public decimal? Rating { get; set; }

    public virtual ICollection<CardItem> CardItems { get; set; } = new List<CardItem>();

    public virtual Category? Category { get; set; }

    public virtual Course? Course { get; set; }
}
