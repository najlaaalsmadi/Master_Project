using backend_Master.DTO;
using backend_Master.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using UglyToad.PdfPig.Graphics;
using PayPalCheckoutSdk.Orders;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _myDbContext;
        private readonly IConfiguration _configuration;

        public UsersController(MyDbContext myDbContext, IConfiguration configuration)
        {
            _myDbContext = myDbContext;
            _configuration = configuration;
        }

        // دالة تسجيل الدخول
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto request)
        {
            var user = _myDbContext.Users
                .SingleOrDefault(u => u.Email == request.Email && u.Role == request.Role);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = CreateJwtToken(user);
            return Ok(new { token });
        }

        //[HttpPost("forget-password")]
        //public async Task<IActionResult> ForgetPassword([FromForm] ForgetPasswordDto request)
        //{
        //    var user = await _myDbContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
        //    if (user == null)
        //    {
        //        return NotFound("User not found.");
        //    }

        //    // توليد OTP
        //    var otp = GenerateOtp();
        //    user.Otp = otp; // تخزين OTP في قاعدة البيانات
        //    await _myDbContext.SaveChangesAsync();

        //    // إرسال OTP عبر البريد الإلكتروني
        //    await SendOtpEmail(user.Email, otp);

        //    return Ok("OTP has been sent to your email.");
        //}
        //[HttpPost("verify-otp")]
        //public async Task<IActionResult> VerifyOtp([FromForm] VerifyOtpDto request)
        //{
        //    var user = await _myDbContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
        //    if (user == null || user.Otp != request.Otp)
        //    {
        //        return BadRequest("Invalid OTP.");
        //    }

        //    // تحديث كلمة المرور
        //    var salt = BCrypt.Net.BCrypt.GenerateSalt();
        //    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, salt);
        //    user.Otp = null; // مسح OTP بعد الاستخدام
        //    await _myDbContext.SaveChangesAsync();

        //    return Ok("Password has been reset successfully.");
        //}

        // دالة التسجيل
        //[HttpPost("signup")]
        //public async Task<IActionResult> Signup([FromForm] UserRegisterDto request)
        //{
        //    // التحقق من وجود المستخدم
        //    if (_myDbContext.Users.Any(u => u.Email == request.Email))
        //    {
        //        return BadRequest(new { message = "User with this email already exists." });
        //    }

        //    // تحقق من صلاحية الدور
        //    if (!new[] { "user", "trainer", "admin" }.Contains(request.Role))
        //    {
        //        return BadRequest(new { message = "Invalid role." });
        //    }

        //    // توليد Salt عشوائي
        //    var salt = BCrypt.Net.BCrypt.GenerateSalt();
        //    // تشفير كلمة المرور مع الـ Salt
        //    string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

        //    // إنشاء كائن المستخدم
        //    var user = new User
        //    {
        //        Name = request.Name,
        //        Email = request.Email,
        //        PasswordHash = passwordHash,
        //        PasswordSalt = salt, // تخزين الـ Salt
        //        Role = request.Role // تعيين الدور من البيانات المدخلة
        //    };

        //    // إضافة المستخدم إلى قاعدة البيانات
        //    _myDbContext.Users.Add(user);
        //    await _myDbContext.SaveChangesAsync();

        //    // إنشاء JWT Token
        //    var token = CreateJwtToken(user);

        //    return Ok(new { token, userId = user.UserId }); // إرجاع الـ userId مع الـ token
        //}
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromForm] UserRegisterDto request)
        {
            // التحقق من وجود المستخدم
            if (_myDbContext.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "User with this email already exists." });
            }

            // تحقق من صلاحية الدور
            if (!new[] { "user", "trainer", "admin" }.Contains(request.Role))
            {
                return BadRequest(new { message = "Invalid role." });
            }

            // توليد Salt عشوائي
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            // تشفير كلمة المرور مع الـ Salt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

            // إنشاء كائن المستخدم
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = salt, // تخزين الـ Salt
                Role = request.Role // تعيين الدور من البيانات المدخلة
            };

            // إضافة المستخدم إلى قاعدة البيانات
            _myDbContext.Users.Add(user);
            await _myDbContext.SaveChangesAsync();

            // إنشاء كائن Card جديد وربط الـ UserId
            var card = new Models.Card
            {
                CardId= user.UserId,
                UserId = user.UserId // تعيين UserId في الكارت
            };

            // إضافة كائن الـ Card إلى قاعدة البيانات
            _myDbContext.Cards.Add(card);
            await _myDbContext.SaveChangesAsync();

            // إنشاء JWT Token
            var token = CreateJwtToken(user);

            return Ok(new { token, userId = user.UserId }); // إرجاع الـ userId مع الـ token
        }





        // إنشاء JWT Token
        private string CreateJwtToken(User user)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [Authorize]
        [HttpGet("protected")]
        public IActionResult GetProtectedData()
        {
            return Ok("This is a protected endpoint.");
        }

        // Generate a 6-digit OTP
        private string GenerateOtp()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] otpBytes = new byte[4];
                rng.GetBytes(otpBytes);
                return (BitConverter.ToUInt32(otpBytes, 0) % 1000000).ToString("D6");
            }
        }

        // Send OTP to email
        private async Task SendOtpEmail(string email, string otp)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("najlaak399@gmail.com", "apxy vexd stma iomf"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("najlaak399@gmail.com"),
                Subject = "Your OTP Code",
                Body = $"Your OTP code is: {otp}",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }

        // API to send OTP
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromForm] ForgetPasswordDto request)
        {
            var user = await _myDbContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Generate OTP
            var otp = GenerateOtp();
            user.Otp = otp; // Store OTP in the database
            await _myDbContext.SaveChangesAsync();

            // Send OTP via email
            await SendOtpEmail(user.Email, otp);

            return Ok("OTP has been sent to your email.");
        }

        // API to verify OTP and reset password
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromForm] VerifyOtpDto request)
        {
            var user = await _myDbContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || user.Otp != request.Otp)
            {
                return BadRequest("Invalid OTP.");
            }

            // Update password
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, salt);
            user.Otp = null; // Clear OTP after usage
            await _myDbContext.SaveChangesAsync();

            return Ok("Password has been reset successfully.");
        }
    
    //private string GenerateOtp()
    //{
    //    using (var rng = new RNGCryptoServiceProvider())
    //    {
    //        byte[] otpBytes = new byte[4];
    //        rng.GetBytes(otpBytes);
    //        return (BitConverter.ToUInt32(otpBytes, 0) % 1000000).ToString("D6");
    //    }
    //}

    //private async Task SendOtpEmail(string email, string otp)
    //{
    //    var smtpClient = new SmtpClient("smtp.gmail.com")
    //    {
    //        Port = 587,
    //        Credentials = new NetworkCredential("election2024jordan@gmail.com", "zwht jwiz ivfr viyt"),
    //        EnableSsl = true,
    //    };

    //    var mailMessage = new MailMessage
    //    {
    //        From = new MailAddress("election2024jordan@gmail.com"),
    //        Subject = "Your OTP Code",
    //        Body = $"Your OTP code is: {otp}",
    //        IsBodyHtml = true,
    //    };
    //    mailMessage.To.Add(email);

    //    await smtpClient.SendMailAsync(mailMessage);
    //}

    [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordWithCurrentDto request)
        {
            // البحث عن المستخدم بناءً على البريد الإلكتروني
            var user = await _myDbContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // التحقق من صحة كلمة المرور الحالية
            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            {
                return BadRequest("Current password is incorrect.");
            }

            // التحقق من تطابق كلمة المرور الجديدة وتأكيدها
            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest("New password and confirmation do not match.");
            }

            // توليد Salt جديد لكلمة المرور الجديدة
            var salt = BCrypt.Net.BCrypt.GenerateSalt();

            // تشفير كلمة المرور الجديدة
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, salt);

            // حفظ التغييرات في قاعدة البيانات
            await _myDbContext.SaveChangesAsync();

            return Ok("Password has been changed successfully.");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            // Retrieve the user by ID
            var user = await _myDbContext.Users
                .Where(u => u.UserId == id)
                .Select(u => new
                {
                    u.UserId,
                    u.Name,
                    u.Email,
                    u.Role,
                    u.CreatedAt,
                    u.Phone,
                    u.BiographicaldetailsCv// Assuming you have a CreatedAt field
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Return user information
            return Ok(user);
        }
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            // استرجاع المستخدم عن طريق البريد الإلكتروني
            var user = await _myDbContext.Users
                .Where(u => u.Email == email)
                .Select(u => new
                {
                    u.UserId,
                    u.Name,
                    u.Email,
                    u.Role,
                    u.CreatedAt,
                    u.Phone,
                    u.BiographicaldetailsCv // assuming you have a BiographicaldetailsCv field
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // إرجاع معلومات المستخدم
            return Ok(user);
        }

        [HttpGet]
[Route("api/Users/email/{email}")]
        public IActionResult GetUserByEmail1(string email)
        {
            var user = _myDbContext.Users.SingleOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(new { studentId = user.UserId }); // Adjust this based on your User model
        }
    }
}
