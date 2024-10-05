namespace backend_Master.DTO
{
    public class CreateEventDto
    {
        public string? EventTitle { get; set; }
        public DateOnly? EventDate { get; set; }
        public TimeOnly? EventTime { get; set; }
        public string? Location { get; set; }
        public int? Participants { get; set; }
        public string? Speaker { get; set; }
        public string? Summary { get; set; }
        public string? Learnings { get; set; }
        public string? Features { get; set; }
        public int? SeatsAvailable { get; set; }
        public int? Topics { get; set; }
        public int? Exams { get; set; }
        public int? Articles { get; set; }
        public int? Certificates { get; set; }

        // Change ImagePath to IFormFile
        public IFormFile? ImagePath { get; set; }

        public string? MapUrl { get; set; }
        public string? ZoomLink { get; set; }
        public string? ZoomPassword { get; set; }
    }

}
