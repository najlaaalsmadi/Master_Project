using System;
using System.Collections.Generic;

namespace backend_Master.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int? OrderId { get; set; }

    public int? CardItemId1 { get; set; }

    public int? CardItemId2 { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public int? ProductId { get; set; }

    public int? EquipmentId { get; set; }

    public virtual CardItemHandmadeProduct? CardItemId1Navigation { get; set; }

    public virtual CardItemLearningEquipment? CardItemId2Navigation { get; set; }

    public virtual Order? Order { get; set; }
}
