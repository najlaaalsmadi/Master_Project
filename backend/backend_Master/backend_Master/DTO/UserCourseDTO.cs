namespace backend_Master.DTO
{
    public class UserCourseDTO
    {
        public int? UserId { get; set; }
        public int? CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.Now; // Default to today
        public int? Progress { get; set; }
        public bool Completed { get; set; }
        public DateTime? EndDate { get; set; } = DateTime.Now.AddYears(1); // Default end date after 1 year
        public int? TrainerId { get; set; }
        //public object PaymentStatus { get; internal set; }
    }
}
