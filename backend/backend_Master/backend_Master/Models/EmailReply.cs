using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class EmailReply
{
    public int Id { get; set; }

    public int EmailMessageId { get; set; }

    public string? ReplyBody { get; set; }

    public DateTime? ReplyDate { get; set; }

    public virtual EmailMessage EmailMessage { get; set; } = null!;
}
