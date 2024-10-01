using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Card
{
    public int CardId { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CardItem> CardItems { get; set; } = new List<CardItem>();

    public virtual User? User { get; set; }
}
