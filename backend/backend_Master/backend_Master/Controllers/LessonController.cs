using backend_Master.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {

        private readonly MyDbContext _context;

        public LessonController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet("{course_id}")]
        public async Task<IActionResult> GetCourseWithLessons(int course_id)
        {
            // جلب الدورة والدروس المرتبطة بها بناءً على course_id
            var courseWithLessons = await _context.Courses
                                                  .Include(c => c.Lessons)  // تضمين الدروس المرتبطة بالدورة
                                                  .FirstOrDefaultAsync(c => c.CourseId == course_id);

            if (courseWithLessons == null)
            {
                return NotFound("Course not found");
            }

            // إعادة الدورة والدروس كـ JSON
            return Ok(new
            {
                course = new
                {
                    courseWithLessons.CourseId,
                    courseWithLessons.Title,
                },
                lessons = courseWithLessons.Lessons.Select(lesson => new
                {
                    lesson.LessonId,
                    lesson.Title,
                    lesson.Content,
                    lesson.VideoUrl,

                    // يمكنك إضافة حقول أخرى للدروس حسب الحاجة
                }).ToList()
            });
        }

        [HttpPut("{lesson_id}")]
        public async Task<IActionResult> UpdateLesson(int lesson_id, [FromBody] Lesson updatedLesson)
        {
            if (lesson_id != updatedLesson.LessonId)
            {
                return BadRequest("Lesson ID mismatch");
            }

            var existingLesson = await _context.Lessons.FindAsync(lesson_id);
            if (existingLesson == null)
            {
                return NotFound("Lesson not found");
            }

            existingLesson.Title = updatedLesson.Title;
            existingLesson.Content = updatedLesson.Content;
            existingLesson.VideoUrl = updatedLesson.VideoUrl;
            existingLesson.Order = updatedLesson.Order;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLesson([FromBody] Lesson newLesson)
        {
            // تحقق مما إذا كانت البيانات المدخلة صحيحة
            if (newLesson == null)
            {
                return BadRequest("Invalid lesson data.");
            }

            // أضف الدرس الجديد إلى قاعدة البيانات
            await _context.Lessons.AddAsync(newLesson);
            await _context.SaveChangesAsync();

            // إرجاع الدرس المضاف مع حالة 201 Created
            return CreatedAtAction(nameof(GetLesson), new { lesson_id = newLesson.LessonId }, newLesson);
        }
        [HttpGet("lesson_id/{lesson_id}")]
        public async Task<IActionResult> GetLesson(int lesson_id)
        {
            var lesson = await _context.Lessons.FindAsync(lesson_id);

            if (lesson == null)
            {
                return NotFound("Lesson not found");
            }

            return Ok(lesson);
        }


        [HttpDelete("{lesson_id}")]
        public async Task<IActionResult> DeleteLesson(int lesson_id)
        {
            var lesson = await _context.Lessons.FindAsync(lesson_id);
            if (lesson == null)
            {
                return NotFound("Lesson not found");
            }

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }

       


    }
}
