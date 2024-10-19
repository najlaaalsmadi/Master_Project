namespace backend_Master.DTO
{

    public class OrderDto
    {
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

}
