using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class CommentsCoursesBefore
{
    public int CommentId { get; set; }

    public string? CommentText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<ResponsesCommentsCoursesBefore> ResponsesCommentsCoursesBefores { get; set; } = new List<ResponsesCommentsCoursesBefore>();
}
