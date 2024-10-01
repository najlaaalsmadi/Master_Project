namespace backend_Master.DTO
{
    public class BookingDTO
    {

        public int? StudentId { get; set; }
        public int? EventId { get; set; }
        public DateOnly? BookingDate { get; set; }
        public string? Status { get; set; }
    }
}
