
using backend_Master.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CoursesController(MyDbContext context)
        {
            _context = context;
        }
        // GET: api/Courses/search?title=example
        // البحث عن دورة باستخدام العنوان
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Course>>> SearchCoursesByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Title is required.");
            }

            var courses = await _context.Courses
                .Where(c => c.Title.Contains(title))
                .ToListAsync();

            if (courses == null || !courses.Any())
            {
                return NotFound("No courses found with the specified title.");
            }

            return Ok(courses);
        }
        // GET: api/Courses/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        {
            var courses = await _context.Courses.ToListAsync();
            return Ok(courses);
        }

        // GET: api/Courses/ratings

[HttpGet("ratings")]
    public async Task<ActionResult<IEnumerable<Course>>> GetCoursesByRatings([FromQuery] int[] ratings)
    {
        if (ratings == null || ratings.Length == 0)
        {
            return BadRequest("Please provide at least one rating.");
        }

        // تصفية الدورات بناءً على التقييمات
        var filteredCourses = await _context.Courses
            .Where(c => ratings.Contains(c.Rating ?? 0))  // تأكد من التعامل مع القيم null في Rating
            .ToListAsync();

        return Ok(filteredCourses);
    }



    // GET: api/Courses/prices?minPrice={minPrice}&maxPrice={maxPrice}
    [HttpGet("prices")]
        public async Task<ActionResult<IEnumerable<object>>> GetPricesInRange(int? minPrice, int? maxPrice)
        {
            // Create a query from the courses in the database
            var pricesQuery = _context.Courses.AsQueryable();

            // Apply minimum price filter if provided
            if (minPrice.HasValue)
            {
                pricesQuery = pricesQuery.Where(c => c.Price >= minPrice.Value);
            }

            // Apply maximum price filter if provided
            if (maxPrice.HasValue)
            {
                pricesQuery = pricesQuery.Where(c => c.Price <= maxPrice.Value);
            }

            // Execute the query and get the result as a list
            var prices = await pricesQuery.ToListAsync();

            // Return the filtered results
            return Ok(prices);
        }




        // POST: api/Courses
        // إنشاء دورة جديدة
        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse([FromBody] Course course)
        {
            if (course == null)
            {
                return BadRequest("Course data is invalid.");
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourseById), new { id = course.CourseId }, course);
        }

        // PUT: api/Courses/{id}
        // تعديل دورة قائمة
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest("Course ID mismatch.");
            }

            var existingCourse = await _context.Courses.FindAsync(id);
            if (existingCourse == null)
            {
                return NotFound();
            }

            // تعديل خصائص الدورة
            existingCourse.Title = course.Title;
            existingCourse.Description = course.Description;
            existingCourse.Price = course.Price;
            existingCourse.Rating = course.Rating;
            existingCourse.Duration = course.Duration;
            existingCourse.AllowedStudents = course.AllowedStudents;
            existingCourse.Syllabus = course.Syllabus;
            existingCourse.Tools = course.Tools;
            existingCourse.StartDate = course.StartDate;
            existingCourse.EndDate = course.EndDate;
            existingCourse.ImageUrl = course.ImageUrl;
            existingCourse.TrainerId = course.TrainerId;
            existingCourse.CategoryId = course.CategoryId;

            _context.Entry(existingCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // DELETE: api/Courses/{id}
        // حذف دورة
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            // Find the course by ID
            var course = await _context.Courses.FindAsync(id);

            // Check if the course exists
            if (course == null)
            {
                return NotFound(); // Return 404 if the course doesn't exist
            }

            // Remove the course from the context
            _context.Courses.Remove(course);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return 200 OK response
            return Ok("تم الحذف بنجاح");
        }


        // التحقق من وجود الدورة
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        // GET: api/Courses/{id}
        // جلب دورة معينة حسب المعرف
        [HttpGet("{id}")]
        public IActionResult GetCourseById(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == id);

            if (course == null)
            {
                Console.WriteLine($"Course with ID {id} not found."); // Add logging
                return NotFound();
            }

            return Ok(course);
        }



        // GET: api/Courses/byCategories?ARRAY
        [HttpGet("byCategories")]
        public IActionResult GetCoursesByCategoryIds([FromQuery] int?[] categoryIds)
        {
            if (categoryIds == null || categoryIds.Length == 0)
            {
                return BadRequest("Category IDs array is required.");
            }

            var courses =  _context.Courses
                .Where(c => categoryIds.Contains(c.CategoryId))
                .ToList();

            if (courses == null || !courses.Any())
            {
                return NotFound("No courses found for the given category IDs.");
            }

            return Ok(courses);
        }



        // GET: api/Courses/random
        // جلب 4 دورات عشوائية
        [HttpGet("random")]
        public async Task<ActionResult<IEnumerable<Course>>> GetRandomCourses()
        {
            var randomCourses = await _context.Courses
                .OrderBy(c => Guid.NewGuid()) // ترتيب عشوائي
                .Take(3) // أخذ 4 دورات فقط
                .ToListAsync();

            return Ok(randomCourses);
        }



        [HttpGet("categoryId/{categoryId}")]
        public IActionResult GetCourseBy(int categoryId)
        {
            // Fetch courses that belong to the given categoryId
            var courses = _context.Courses.Where(c => c.CategoryId == categoryId).ToList();

            // Check if the list is empty
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }

            return Ok(courses);
        }










    }
}
