using backend_Master.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private readonly MyDbContext _context;

            public TrainerController(MyDbContext context)
            {
                _context = context;
            }

            // GET: api/Trainers/all
            // عرض جميع المدربين
            [HttpGet("all")]
            public async Task<ActionResult<IEnumerable<Trainer>>> GetAllTrainers()
            {
                var trainers = await _context.Trainers.ToListAsync();
                return Ok(trainers);
            }

            // GET: api/Trainers/{id}
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

            // POST: api/Trainers
            // إنشاء مدرب جديد
            [HttpPost]
            public async Task<ActionResult<Trainer>> CreateTrainer([FromBody] Trainer trainer)
            {
                if (trainer == null)
                {
                    return BadRequest("Invalid trainer data.");
                }

                _context.Trainers.Add(trainer);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTrainerById), new { id = trainer.TrainerId }, trainer);
            }

            // PUT: api/Trainers/{id}
            // تعديل بيانات مدرب بناءً على ID
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

                // تعديل خصائص المدرب
                existingTrainer.Bio = trainer.Bio;
                existingTrainer.Experience = trainer.Experience;
                existingTrainer.Specialization = trainer.Specialization;
                existingTrainer.UserId = trainer.UserId;

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

            // DELETE: api/Trainers/{id}
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

                return NoContent();
            }

            // التحقق من وجود مدرب بناءً على ID
            private bool TrainerExists(int id)
            {
                return _context.Trainers.Any(e => e.TrainerId == id);
            }

            // GET: api/Trainers/search?specialization=example
            // البحث عن مدرب بناءً على التخصص
            [HttpGet("search")]
            public async Task<ActionResult<IEnumerable<Trainer>>> SearchTrainersBySpecialization(string specialization)
            {
                if (string.IsNullOrWhiteSpace(specialization))
                {
                    return BadRequest("Specialization is required.");
                }

                var trainers = await _context.Trainers
                    .Where(t => t.Specialization.Contains(specialization))
                    .ToListAsync();

                if (trainers == null || !trainers.Any())
                {
                    return NotFound("No trainers found with the specified specialization.");
                }

                return Ok(trainers);
            }
        }
    }

