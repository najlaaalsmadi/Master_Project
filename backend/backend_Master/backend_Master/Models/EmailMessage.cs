using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class EmailMessage
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime DateSent { get; set; }

    public virtual ICollection<EmailReply> EmailReplies { get; set; } = new List<EmailReply>();
}
