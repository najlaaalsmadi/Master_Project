using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class CardItemLearningEquipment
{
    public int CardItemId { get; set; }

    public int? CardId { get; set; }

    public int? EquipmentId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual Card? Card { get; set; }

    public virtual LearningEquipment? Equipment { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
