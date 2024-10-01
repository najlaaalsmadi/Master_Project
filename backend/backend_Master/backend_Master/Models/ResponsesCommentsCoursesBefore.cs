using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class ResponsesCommentsCoursesBefore
{
    public int ResponseId { get; set; }

    public int? CommentId { get; set; }

    public string? ResponseText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Name { get; set; }

    public virtual CommentsCoursesBefore? Comment { get; set; }
}
