using backend_Master.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsCoursesBeforeController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CommentsCoursesBeforeController(MyDbContext context)
        {
            _context = context;
        }

        // إرجاع جميع التعليقات
        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            try
            {
                var comments = await _context.CommentsCoursesBefores.ToListAsync();
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }
        [HttpGet("GetCommentsRandom")]
        public async Task<IActionResult> GetCommentsRandom()
        {
            try
            {
                // الحصول على تعليقين عشوائيين
                var comments = await _context.CommentsCoursesBefores
                                             .OrderBy(c => Guid.NewGuid()) // ترتيب عشوائي
                                             .Take(2) // عرض تعليقين فقط
                                             .ToListAsync();

                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }

        // إضافة تعليق جديد
        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody] CommentsCoursesBefore comment)
        {
            if (comment == null)
            {
                return BadRequest("Invalid comment data.");
            }

            try
            {
                _context.CommentsCoursesBefores.Add(comment);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetComment), new { id = comment.CommentId }, comment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving the comment: {ex.Message}");
            }
        }


        // الحصول على تعليق واحد حسب الـID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetComment(int id)
        {
            try
            {
                var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == id);

                if (comment == null)
                {
                    return NotFound($"Comment with ID {id} not found.");
                }

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving comment: {ex.Message}");
            }
        }
    }
}
