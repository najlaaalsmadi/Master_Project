using System.ComponentModel.DataAnnotations;

namespace backend_Master.DTO
{
    public class TrainerRegisterDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "CV file is required.")]
        public IFormFile? CvFile { get; set; }

        public IFormFile? ImageProfile { get; set; } // يمكن إضافة خصائص لتخزين أسماء الملفات إذا لزم الأمر

        public string? Bio { get; set; }
        public string? Experience { get; set; }
        public string? Specialization { get; set; }
        public string? JobTitle { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
    }
}
