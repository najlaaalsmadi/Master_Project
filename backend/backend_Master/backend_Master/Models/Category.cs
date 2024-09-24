using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
