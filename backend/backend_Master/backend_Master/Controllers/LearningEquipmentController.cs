using backend_Master.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningEquipmentController : ControllerBase
    {
        private readonly MyDbContext _context;

        public LearningEquipmentController(MyDbContext context)
        {
            _context = context;
        }
        // GET: api/LearningEquipment/category/{category_id}
        [HttpGet("category/{category_id}")]
        public async Task<ActionResult<IEnumerable<LearningEquipment>>> GetByCategory(int category_id)
        {
            var equipmentList = await _context.LearningEquipments
                .Where(e => e.CategoryId == category_id)
                .ToListAsync();

            if (equipmentList == null || equipmentList.Count == 0)
            {
                return NotFound();
            }

            return Ok(equipmentList);
        }
        // GET: api/LearningEquipment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LearningEquipment>>> GetAll()
        {
            return await _context.LearningEquipments.ToListAsync();
        }

        // GET: api/LearningEquipment/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LearningEquipment>> GetById(int id)
        {
            var equipment = await _context.LearningEquipments.FindAsync(id);

            if (equipment == null)
            {
                return NotFound();
            }

            return equipment;
        }

        // GET: api/LearningEquipment/random
        [HttpGet("random")]
        public async Task<ActionResult<IEnumerable<LearningEquipment>>> GetRandom()
        {
            return await _context.LearningEquipments
                .OrderBy(e => Guid.NewGuid())
                .Take(3)
                .ToListAsync();
        }

        // POST: api/LearningEquipment
        [HttpPost]
        public async Task<ActionResult<LearningEquipment>> Post(LearningEquipment equipment)
        {
            _context.LearningEquipments.Add(equipment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = equipment.EquipmentId }, equipment);
        }

        // PUT: api/LearningEquipment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, LearningEquipment equipment)
        {
            if (id != equipment.EquipmentId)
            {
                return BadRequest();
            }

            _context.Entry(equipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentExists(id))
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

        // DELETE: api/LearningEquipment/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var equipment = await _context.LearningEquipments.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }

            _context.LearningEquipments.Remove(equipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EquipmentExists(int id)
        {
            return _context.LearningEquipments.Any(e => e.EquipmentId == id);
        }







        // GET: api/LearningEquipment/byCategories?categoryIds={id1,id2,...}
        // GET: api/LearningEquipment/byCategories
        [HttpGet("byCategories")]
        public async Task<ActionResult<IEnumerable<LearningEquipment>>> GetLearningEquipmentByCategoryIds([FromQuery] List<int> categoryIds)
        {
            if (categoryIds == null || categoryIds.Count == 0)
            {
                return BadRequest("مطلوب تقديم مصفوفة من معرفات الفئات.");
            }

            // تصفية المعدات التعليمية بناءً على معرفات الفئات المقدمة
            var equipment = await _context.LearningEquipments
                .Where(e => e.CategoryId.HasValue && categoryIds.Contains(e.CategoryId.Value)) // التأكد من أن CategoryId ليس فارغًا
                .ToListAsync();

            if (equipment == null || !equipment.Any())
            {
                return NotFound("لم يتم العثور على المعدات التعليمية للفئات المحددة.");
            }

            return Ok(equipment);
        }


        // GET: api/LearningEquipment/ratings
        [HttpGet("ratings")]
        public async Task<ActionResult<IEnumerable<LearningEquipment>>> GetLearningEquipmentByRatings([FromQuery] decimal[] ratings)
        {
            if (ratings == null || ratings.Length == 0)
            {
                return BadRequest("يرجى تقديم تقييم واحد على الأقل.");
            }

            var filteredEquipment = await _context.LearningEquipments
                .Where(e => ratings.Contains(e.Rating ?? 0)) // تأكد من التعامل مع القيم null في Rating
                .ToListAsync();

            return Ok(filteredEquipment);
        }
        // GET: api/LearningEquipment/prices?minPrice={minPrice}&maxPrice={maxPrice}
        [HttpGet("prices")]
        public async Task<ActionResult<IEnumerable<LearningEquipment>>> GetLearningEquipmentByPriceRange(decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.LearningEquipments.AsQueryable();

            if (minPrice.HasValue)
            {
                query = query.Where(e => e.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(e => e.Price <= maxPrice.Value);
            }

            var filteredEquipment = await query.ToListAsync();
            return Ok(filteredEquipment);
        }












    }

}
