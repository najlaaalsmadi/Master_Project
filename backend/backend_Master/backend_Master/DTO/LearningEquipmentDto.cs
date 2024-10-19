using System.ComponentModel.DataAnnotations;

namespace backend_Master.DTO
{
    public class LearningEquipmentDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }  // Ensure this is a valid foreign key

        public int CourseId { get; set; }    // Ensure this is a valid foreign key

        public IFormFile Image1 { get; set; }  // Optional: First Image File

        public IFormFile Image2 { get; set; }  // Optional: Second Image File

        public IFormFile Image3 { get; set; }  // Optional: Third Image File
    }

}
