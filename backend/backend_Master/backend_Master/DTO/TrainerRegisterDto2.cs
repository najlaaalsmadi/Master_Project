
namespace backend_Master.DTO
{
    public class TrainerRegisterDto2
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string? Bio { get; set; }
        public string? Experience { get; set; }
        public string? Specialization { get; set; }
        public string? Phone { get; set; }
        public string? JobTitle { get; set; }

        // Property to hold the profile image file
        public IFormFile ImageProfile { get; set; }

        // Property to hold the CV file
        public IFormFile Cv { get; set; }
    }
}