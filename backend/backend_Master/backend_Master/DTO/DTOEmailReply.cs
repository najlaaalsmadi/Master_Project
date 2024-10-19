using System.ComponentModel.DataAnnotations;

namespace backend_Master.DTO
{
    public class DTOEmailReply
    {
        [Required]
        public  string ?Name { get; set; }

        [Required]
        [EmailAddress]
        public string ?Email { get; set; }

        [Required]
        public string ?ReplyBody { get; set; }
    }
}
