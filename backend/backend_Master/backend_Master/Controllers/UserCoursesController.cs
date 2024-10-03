using backend_Master.DTO;
using backend_Master.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPalCheckoutSdk.Orders;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCoursesController : ControllerBase
    {

        private readonly MyDbContext _context;

        public UserCoursesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/UserCourses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCourse>>> GetUserCourses()
        {
            return await _context.UserCourses.ToListAsync();
        }

        // GET: api/UserCourses/5
        [HttpGet("{id}")]
        public IActionResult GetUserCourse(int id)
        {
            // استرجاع بيانات الدورة للمستخدم بناءً على EnrollmentId
            var userCourse = _context.UserCourses
                .Include(uc => uc.Course) // تضمين معلومات الدورة
                .Include(uc => uc.User)    // تضمين معلومات المستخدم
                .FirstOrDefault(uc => uc.EnrollmentId == id); // البحث عن التسجيل

            // تحقق إذا كانت بيانات التسجيل غير موجودة
            if (userCourse == null)
            {
                return NotFound(); // إرجاع 404 إذا لم يتم العثور على التسجيل
            }

            // إعداد كائن يحتوي فقط على البيانات المطلوبة
            var result = new
            {
                userCourse.EnrollmentId,        // معرف التسجيل
                CourseName = userCourse.Course.Title, // اسم الدورة
                CourseId = userCourse.CourseId,        // رقم الدورة
                UserName = userCourse.User.Name,       // اسم المستخدم
                EnrollmentDate = userCourse.EnrollmentDate // تاريخ التسجيل
            };

            return Ok(result); // إرجاع البيانات مع حالة 200
        }
        [HttpGet("user/{userId}")]
        public IActionResult GetUserCourses(int userId)
        {
            // استرجاع بيانات الدورات للمستخدم بناءً على UserId
            var userCourses = _context.UserCourses
                .Include(uc => uc.Course) // تضمين معلومات الدورة
                .Include(uc => uc.Trainer) // تضمين معلومات المدرب
                .Where(uc => uc.UserId == userId) // تصفية الدورات بناءً على UserId
                .Select(uc => new
                {
                    uc.EnrollmentId,             // معرف التسجيل
                    CourseName = uc.Course.Title, // اسم الدورة
                    CourseId = uc.CourseId,       // رقم الدورة
                    price = uc.Course.Price,       // رقم الدورة
                    img = uc.Course.ImageUrl,
                    UserName = uc.User.Name,      // اسم المستخدم
                    EnrollmentDate = uc.EnrollmentDate, // تاريخ التسجيل
                    TrainerId = uc.TrainerId,
                    TrainerName = uc.Trainer.Name  // اسم المدرب
                                                   // رقم المدرب
                })
                .ToList(); // تحويل النتيجة إلى قائمة

            // تحقق إذا كانت بيانات الدورات غير موجودة
            if (!userCourses.Any())
            {
                return NotFound(); // إرجاع 404 إذا لم يتم العثور على أي دورات
            }

            return Ok(userCourses); // إرجاع البيانات مع حالة 200
        }
        [HttpPost("CreateUserCourse")]
        public IActionResult CreateUserCourse([FromBody] UserCourseDTO userCourseDto)
        {
            if (userCourseDto == null)
            {
                return BadRequest("بيانات غير صحيحة.");
            }

            var userCourse = new UserCourse
            {
                UserId = userCourseDto.UserId,
                CourseId = userCourseDto.CourseId,
                EnrollmentDate = DateTime.Now,
                Progress = 0,
                Completed = false,
            };

            _context.UserCourses.Add(userCourse);
            _context.SaveChanges();

            return CreatedAtAction(nameof(CreateUserCourse), new { id = userCourse.EnrollmentId }, userCourse);
        }

        // POST: api/UserCourses
        [HttpPost]
        public async Task<ActionResult<UserCourse>> PostUserCourse([FromForm] UserCourse userCourse)
        {
            if (ModelState.IsValid)
            {
                _context.UserCourses.Add(userCourse);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUserCourse", new { id = userCourse.EnrollmentId }, userCourse);
            }
            return BadRequest(ModelState);
        }


        // PUT: api/UserCourses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserCourse(int id, UserCourse userCourse)
        {
            if (id != userCourse.EnrollmentId)
            {
                return BadRequest();
            }

            _context.Entry(userCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCourseExists(id))
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

        // DELETE: api/UserCourses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserCourse(int id)
        {
            var userCourse = await _context.UserCourses.FindAsync(id);
            if (userCourse == null)
            {
                return NotFound();
            }

            _context.UserCourses.Remove(userCourse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserCourseExists(int id)
        {
            return _context.UserCourses.Any(e => e.EnrollmentId == id);
        }
    }

}

