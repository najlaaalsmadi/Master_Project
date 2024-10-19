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


        [HttpPost("{emailMessageId}/reply")]
        public async Task<IActionResult> ReplyToEmail(int emailMessageId, [FromForm] DTOEmailReply replyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("البيانات المدخلة غير صالحة");
            }

            var emailMessage = await _context.EmailMessages.FindAsync(emailMessageId);

            if (emailMessage == null)
            {
                return NotFound("لم يتم العثور على الرسالة.");
            }

            try
            {
                // إعداد عنوان و محتوى الرد
                var subject = $"رد جديد على رسالتك: لموقع ايديا ميديا";
                var message = $"{replyDto.ReplyBody}\n\nمن: {replyDto.Name}\nالبريد الإلكتروني: {replyDto.Email}";

                // إرسال الرد عبر البريد الإلكتروني
                await _emailService.SendEmailAsync(emailMessage.Email, subject, message);

                // تخزين الرد في قاعدة البيانات
                var emailReply = new EmailReply
                {
                    EmailMessageId = emailMessageId,
                    ReplyBody = replyDto.ReplyBody,
                    ReplyDate = DateTime.Now
                };

                _context.EmailReplies.Add(emailReply);
                await _context.SaveChangesAsync();

                // إرجاع الرد مع الاسم والبريد الإلكتروني
                return Ok(new
                {
                    message = "تم إرسال الرد وتخزينه بنجاح!",
                    name = replyDto.Name,
                    email = replyDto.Email
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"حدث خطأ أثناء إرسال أو تخزين الرد: {ex.Message}");
            }
        }

        [HttpGet("{emailMessageId}/replies")]
        public async Task<ActionResult<IEnumerable<EmailReply>>> GetRepliesByEmailId(int emailMessageId)
        {
            var emailMessage = await _context.EmailMessages.FindAsync(emailMessageId);

            if (emailMessage == null)
            {
                return NotFound("لم يتم العثور على الرسالة.");
            }

            var replies = await _context.EmailReplies
                .Where(r => r.EmailMessageId == emailMessageId)
                .ToListAsync();

            return Ok(replies);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var message = await _context.EmailMessages.FindAsync(id);

            if (message == null)
            {
                return NotFound(new { message = "الرسالة غير موجودة." });
            }

            // Return the message details
            return Ok(new
            {
                name = message.Name,
                email = message.Email,
                messageBody = message.Message,
                dateSent = message.DateSent
            });
        }

        // Controller for Email
        [HttpGet("{id}/replies123")]
        public IActionResult GetReplies(int id)
        {
            var replies = _context.EmailReplies
                                    .Where(r => r.EmailMessageId == id)
                                    .ToList();

            if (replies == null || !replies.Any())
            {
                return NotFound(new { message = "لا يوجد ردود لهذه الرسالة" });
            }

            return Ok(replies);
        }


        // جلب جميع الرسائل
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<EmailMessage>>> GetAllEmails()
        {
            var messages = await _context.EmailMessages.ToListAsync();
            return Ok(messages);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmailMessage(int id)
        {
            var emailMessage = await _context.EmailMessages.FindAsync(id);
            if (emailMessage == null)
            {
                return NotFound(new { message = "Email not found" }); // إرجاع رسالة إذا لم يتم العثور على الإيميل
            }

            _context.EmailMessages.Remove(emailMessage);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Email deleted successfully" }); // إرجاع رسالة نجاح
        }
    }
}
