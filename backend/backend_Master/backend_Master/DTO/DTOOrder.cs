namespace backend_Master.DTO
{
    public class DTOOrder
    {
        public class OrderDTO
        {
            public int OrderId { get; set; }
            public string UserName { get; set; }
            public decimal TotalAmount { get; set; }
            public string Status { get; set; }
            public DateTime? CreatedAt { get; set; }
            public List<OrderItemDTO> OrderItems { get; set; }
        }

        public class OrderItemDTO
        {
            public int OrderItemId { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

    }
}
