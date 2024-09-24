using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int? UserId { get; set; }

    public string? ItemType { get; set; }

    public int? ItemId { get; set; }

    public string? CommentText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
