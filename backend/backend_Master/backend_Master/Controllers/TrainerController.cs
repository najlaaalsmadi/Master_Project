using backend_Master.DTO;
using backend_Master.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO; // Ensure this is included
using UglyToad.PdfPig;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;

        public TrainerController(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // عرض جميع المدربين
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetAllTrainers()
        {
            var trainers = await _context.Trainers.ToListAsync();
            return Ok(trainers);
        }

        // جلب مدرب بناءً على معرف
        [HttpGet("{id}")]
        public async Task<ActionResult<Trainer>> GetTrainerById(int id)
        {
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.TrainerId == id);
            if (trainer == null)
            {
                return NotFound($"Trainer with ID {id} not found.");
            }
            return Ok(trainer);
        }

        // رفع ملف PDF واستخراج البيانات منه
        [HttpPost("upload")]
        public async Task<IActionResult> UploadPdf(IFormFile pdfFile, [FromForm] string name, [FromForm] string email, [FromForm] string password)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // استخراج البيانات من PDF
            var extractedData = await ExtractDataFromPdf(pdfFile);

            // التحقق من البيانات المستخرجة
            if (extractedData == null || extractedData.Count < 5)
            {
                Console.WriteLine($"Number of lines extracted: {extractedData?.Count}");
                return BadRequest("Invalid data extracted from PDF.");
            }

            // تعبئة بيانات TrainerRegisterDto
            var trainerRequest = new TrainerRegisterDto2
            {
                Name = name,
                Email = email,
                Password = password,
                Bio = extractedData.GetValueOrDefault("السيرة الذاتية") ?? "Bio not available",
                Experience = extractedData.GetValueOrDefault("الخبرة") ?? "Experience not available",
                Specialization = extractedData.GetValueOrDefault("التخصص") ?? "Specialization not available",
                JobTitle = extractedData.GetValueOrDefault("العنوان") ?? "Job Title not available",
                Phone = extractedData.GetValueOrDefault("الهاتف") ?? "Phone not available"
            };

            // تسجيل المدرب
            var signupResult = await SignupTrainer1(trainerRequest);
            if (signupResult is BadRequestObjectResult)
            {
                return signupResult;
            }

            return Ok("Trainer has been signed up successfully.");
        }

        // تسجيل المدرب الجديد لـ TrainerRegisterDto
        private async Task<IActionResult> SignupTrainer1(TrainerRegisterDto2 request)
        {
            if (_context.Trainers.Any(t => t.Email == request.Email))
            {
                return BadRequest(new { message = "مدرب بهذا البريد الإلكتروني موجود بالفعل." });
            }

            // إنشاء Salt وكلمة مرور مشفرة
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

            var trainer = new Trainer
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = salt,
                Bio = request.Bio,
                Experience = request.Experience,
                Specialization = request.Specialization,
                JobTitle = request.JobTitle,
                Phone = request.Phone
            };

            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();

            // إنشاء JWT Token
            var token = CreateJwtToken(trainer);

            return Ok(new { token, trainerId = trainer.TrainerId });
        }

        // استخراج النصوص من PDF باستخدام PdfPig
        private async Task<Dictionary<string, string>> ExtractDataFromPdf(IFormFile pdfFile)
        {
            Dictionary<string, string> extractedData = new Dictionary<string, string>();

            using (var stream = new MemoryStream())
            {
                await pdfFile.CopyToAsync(stream);
                stream.Position = 0;

                using (var pdfDocument = PdfDocument.Open(stream))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var page in pdfDocument.GetPages())
                    {
                        sb.Append(page.Text); // تجميع النص من جميع الصفحات
                    }

                    string fullText = sb.ToString();

                    // تقسيم النص بناءً على الكلمات المفتاحية
                    extractedData["الاسم"] = ExtractSection(fullText, "الاسم:", "السيرة الذاتية:");
                    extractedData["السيرة الذاتية"] = ExtractSection(fullText, "السيرة الذاتية:", "الخبرة:");
                    extractedData["الخبرة"] = ExtractSection(fullText, "الخبرة:", "التخصص:");
                    extractedData["التخصص"] = ExtractSection(fullText, "التخصص:", "العنوان:");
                    extractedData["العنوان"] = ExtractSection(fullText, "العنوان:", "الهاتف:");
                    extractedData["الهاتف"] = ExtractSection(fullText, "الهاتف:", ".");
                }
            }

            return extractedData;
        }

        // استخراج جزء معين من النص
        private string ExtractSection(string text, string startKeyword, string endKeyword)
        {
            var startIndex = text.IndexOf(startKeyword);
            if (startIndex == -1) return "";

            startIndex += startKeyword.Length;
            var endIndex = text.IndexOf(endKeyword, startIndex);
            if (endIndex == -1) return text.Substring(startIndex).Trim();

            return text.Substring(startIndex, endIndex - startIndex).Trim();
        }

        // إنشاء JWT Token
        private string CreateJwtToken(Trainer trainer)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, trainer.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, trainer.Role)
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

        // تعديل بيانات المدرب بناءً على ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainer(int id, [FromBody] Trainer trainer)
        {
            if (id != trainer.TrainerId)
            {
                return BadRequest("Trainer ID mismatch.");
            }

            var existingTrainer = await _context.Trainers.FindAsync(id);
            if (existingTrainer == null)
            {
                return NotFound($"Trainer with ID {id} not found.");
            }

            existingTrainer.Bio = trainer.Bio;
            existingTrainer.Experience = trainer.Experience;
            existingTrainer.Specialization = trainer.Specialization;

            _context.Entry(existingTrainer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // حذف مدرب بناءً على ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound($"Trainer with ID {id} not found.");
            }

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();

            return Ok($"Trainer with ID {id} deleted.");
        }

        private bool TrainerExists(int id)
        {
            return _context.Trainers.Any(e => e.TrainerId == id);
        }

        // تسجيل مدرب جديد باستخدام بيانات POST مباشرة
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromForm] TrainerRegisterDto2 request)
        {
            // Check if the trainer already exists
            if (_context.Trainers.Any(t => t.Email == request.Email))
            {
                return BadRequest(new { message = "Trainer with this email already exists." });
            }

            // Validate the role
            if (!new[] { "trainer", "admin" }.Contains(request.Role))
            {
                return BadRequest(new { message = "Invalid role." });
            }

            // Generate a random salt
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            // Hash the password with the salt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

            // Create a new Trainer object
            var trainer = new Trainer
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = salt,
                Role = request.Role,
                Bio = request.Bio,
                Experience = request.Experience,
                Specialization = request.Specialization,
                Phone = request.Phone,
                JobTitle = request.JobTitle,
            };

            // Set the upload folder path
            var uploadsFolder = @"C:\Users\Orange\Desktop\Master_Project\backend\image";
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Handle profile image upload
            if (request.ImageProfile != null && request.ImageProfile.Length > 0)
            {
                var profileFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageProfile.FileName);
                var profileFilePath = Path.Combine(uploadsFolder, profileFileName);

                using (var stream = new FileStream(profileFilePath, FileMode.Create))
                {
                    await request.ImageProfile.CopyToAsync(stream);
                }

                trainer.ImageProfile = profileFileName; // Store the filename in the database
            }

            // Handle CV upload
            if (request.Cv != null && request.Cv.Length > 0)
            {
                var cvFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.Cv.FileName);
                var cvFilePath = Path.Combine(uploadsFolder, cvFileName);

                using (var stream = new FileStream(cvFilePath, FileMode.Create))
                {
                    await request.Cv.CopyToAsync(stream);
                }

                trainer.Cv = cvFileName; // Store the CV filename in the database
            }

            // Add the trainer to the database
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();

            // Create JWT Token
            var token = CreateJwtToken(trainer);

            return Ok(new { token, trainerId = trainer.TrainerId }); // Return the trainerId with the token
        }



    }
}
