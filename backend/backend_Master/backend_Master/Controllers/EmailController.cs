using backend_Master.DTO;
using backend_Master.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly EmailService _emailService;

        public EmailController(MyDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // إرسال بريد إلكتروني وحفظه في قاعدة البيانات
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromForm] DTOEmailMessage emailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("البيانات المدخلة غير صالحة");
            }

            var subject = $"رسالة جديدة من {emailDto.Name}";
            var message = $"{emailDto.Message}\n\nمن: {emailDto.Name}\nالبريد الإلكتروني: {emailDto.Email}";

            try
            {
                // إرسال البريد الإلكتروني
                await _emailService.SendEmailAsync("admin@example.com", subject, message);

                // تخزين الرسالة في قاعدة البيانات
                var emailMessage = new EmailMessage
                {
                    Name = emailDto.Name,
                    Email = emailDto.Email,
                    Message = emailDto.Message,
                    DateSent = DateTime.Now
                };

                _context.EmailMessages.Add(emailMessage);
                await _context.SaveChangesAsync();

                return Ok(new { message = "تم إرسال البريد الإلكتروني وتخزينه بنجاح!" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"حدث خطأ أثناء إرسال أو تخزين البريد: {ex.Message}");
            }
        }


        // استقبال بريد إلكتروني وحفظه في قاعدة البيانات
        [HttpPost("receive")]
        public async Task<ActionResult<EmailMessage>> ReceiveEmail([FromBody] EmailMessage message)
        {
            if (message == null)
            {
                return BadRequest("بيانات الرسالة غير صالحة.");
            }

            message.DateSent = DateTime.Now;

            _context.EmailMessages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmailById), new { id = message.Id }, message);
        }

        // جلب رسالة بناءً على المعرف
        [HttpGet("{id}")]
        public async Task<ActionResult<EmailMessage>> GetEmailById(int id)
        {
            var message = await _context.EmailMessages.FindAsync(id);

            if (message == null)
            {
                return NotFound("لم يتم العثور على الرسالة.");
            }

            return Ok(message);
        }

        // جلب جميع الرسائل
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<EmailMessage>>> GetAllEmails()
        {
            var messages = await _context.EmailMessages.ToListAsync();
            return Ok(messages);
        }
    }
}
