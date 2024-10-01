using System.ComponentModel.DataAnnotations;

namespace backend_Master.DTO
{


    public class UserRegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // إضافة خاصية الدور

    }

    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

}


