using backend_Master.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_Master.DTO
{
    public class VerifyOtpDto
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; } // كلمة المرور الجديدة
    }

   

}
