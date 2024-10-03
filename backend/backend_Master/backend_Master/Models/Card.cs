using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class Card
{
    public int CardId { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CardItemHandmadeProduct> CardItemHandmadeProducts { get; set; } = new List<CardItemHandmadeProduct>();

    public virtual ICollection<CardItemLearningEquipment> CardItemLearningEquipments { get; set; } = new List<CardItemLearningEquipment>();

    public virtual User? User { get; set; }
}
