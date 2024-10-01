using Microsoft.AspNetCore.Mvc;
using backend_Master.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_Master.DTO;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsletterController : ControllerBase
    {
        private readonly MyDbContext _context;

        public NewsletterController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Newsletter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Newsletter>>> GetAllNewsletters()
        {
            return await _context.Set<Newsletter>().ToListAsync();
        }

        // GET: api/Newsletter/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Newsletter>> GetNewsletter(int id)
        {
            var newsletter = await _context.Set<Newsletter>().FindAsync(id);

            if (newsletter == null)
            {
                return NotFound();
            }

            return newsletter;
        }
        [HttpPost("DTONewsletter")]
        public async Task<IActionResult> Newsletter([FromForm] DTONewsletter request)
        {
            // التحقق من وجود البريد الإلكتروني مسبقًا
            if (_context.Newsletters.Any(n => n.Email == request.Email))
            {
                return BadRequest(new { message = "This email is already subscribed." });
            }

            // إنشاء كائن النشرة الإخبارية
            var newsletter = new Newsletter
            {
                Email = request.Email,
                SubscribedAt = DateTime.UtcNow // تخزين وقت الاشتراك الحالي
            };

            // إضافة النشرة إلى قاعدة البيانات
            _context.Newsletters.Add(newsletter);
            _context.SaveChangesAsync();

            return Ok(new { message = "Subscribed successfully!", newsletterId = newsletter.NewsletterId });
        }
    

    // PUT: api/Newsletter/5
    [HttpPut("{id}")]
        public async Task<IActionResult> PutNewsletter(int id, Newsletter newsletter)
        {
            if (id != newsletter.NewsletterId)
            {
                return BadRequest();
            }

            _context.Entry(newsletter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsletterExists(id))
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

        // DELETE: api/Newsletter/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewsletter(int id)
        {
            var newsletter = await _context.Set<Newsletter>().FindAsync(id);
            if (newsletter == null)
            {
                return NotFound();
            }

            _context.Set<Newsletter>().Remove(newsletter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NewsletterExists(int id)
        {
            return _context.Set<Newsletter>().Any(e => e.NewsletterId == id);
        }
    }
}
