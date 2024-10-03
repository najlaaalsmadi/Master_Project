namespace backend_Master.DTO
{
    public class PaymentsCourseDto
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string? CardName { get; set; }
        public decimal Amount { get; set; }
        public string ?PaymentMethod { get; set; }
        public string ?FirstName { get; set; }
        public string ?LastName { get; set; }
        public string ?AddressLine1 { get; set; }
        public string ?AddressLine2 { get; set; }
        public string ?City { get; set; }
        public string ?State { get; set; }
        public string ?ZipCode { get; set; }
        public string ?Country { get; set; }
        public string ?CountryCallingCode { get; set; }
        public string ?PhoneNumber { get; set; }
    }


}
