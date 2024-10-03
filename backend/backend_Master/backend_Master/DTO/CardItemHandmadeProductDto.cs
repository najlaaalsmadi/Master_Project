namespace backend_Master.Models
{
    public class CardItemHandmadeProductDto
    {
        public int? CardId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTime? AddedAt { get; set; }
    }
}
