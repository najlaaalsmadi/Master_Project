using backend_Master.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Handmade_ProductsController : ControllerBase
    {

        private readonly MyDbContext _context;

        public Handmade_ProductsController(MyDbContext context)
        {
            _context = context;
        }
        // GET: api/LearningEquipment/category/{category_id}
        [HttpGet("category/{category_id}")]
        public async Task<ActionResult<IEnumerable<HandmadeProduct>>> GetByCategory(int category_id)
        {
            var HandmadeProductList = await _context.HandmadeProducts
                .Where(e => e.CategoryId == category_id)
                .ToListAsync();

            if (HandmadeProductList == null || HandmadeProductList.Count == 0)
            {
                return NotFound();
            }

            return Ok(HandmadeProductList);
        }
        // GET: api/LearningEquipment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HandmadeProduct>>> GetAll()
        {
            return await _context.HandmadeProducts.ToListAsync();
        }

        // GET: api/LearningEquipment/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<HandmadeProduct>> GetById(int id)
        {
            var HandmadeProduct = await _context.HandmadeProducts.FindAsync(id);

            if (HandmadeProduct == null)
            {
                return NotFound();
            }

            return HandmadeProduct;
        }

        // GET: api/LearningEquipment/random
        [HttpGet("random")]
        public async Task<ActionResult<IEnumerable<HandmadeProduct>>> GetRandom()
        {
            return await _context.HandmadeProducts
                .OrderBy(e => Guid.NewGuid())
                .Take(3)
                .ToListAsync();
        }

        // POST: api/LearningEquipment
        [HttpPost]
        public async Task<ActionResult<HandmadeProduct>> Post(HandmadeProduct Handmade)
        {
            _context.HandmadeProducts.Add(Handmade);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = Handmade.ProductId }, Handmade);
        }

        // PUT: api/LearningEquipment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, HandmadeProduct Handmade)
        {
            if (id != Handmade.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(Handmade).State = EntityState.Modified;

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
            var Handmade = await _context.HandmadeProducts.FindAsync(id);
            if (Handmade == null)
            {
                return NotFound();
            }

            _context.HandmadeProducts.Remove(Handmade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EquipmentExists(int id)
        {
            return _context.HandmadeProducts.Any(e => e.ProductId == id);
        }







        // GET: api/LearningEquipment/byCategories?categoryIds={id1,id2,...}
        // GET: api/LearningEquipment/byCategories
        [HttpGet("byCategories")]
        public async Task<ActionResult<IEnumerable<HandmadeProduct>>> GetHandmadeProductsByCategoryIds([FromQuery] List<int> categoryIds)
        {
            if (categoryIds == null || categoryIds.Count == 0)
            {
                return BadRequest("مطلوب تقديم مصفوفة من معرفات الفئات.");
            }

            // تصفية المعدات التعليمية بناءً على معرفات الفئات المقدمة
            var Handmade = await _context.HandmadeProducts
                .Where(e => e.CategoryId.HasValue && categoryIds.Contains(e.CategoryId.Value)) // التأكد من أن CategoryId ليس فارغًا
                .ToListAsync();

            if (Handmade == null || !Handmade.Any())
            {
                return NotFound("لم يتم العثور على المعدات التعليمية للفئات المحددة.");
            }

            return Ok(Handmade);
        }


        // GET: api/LearningEquipment/ratings
        [HttpGet("ratings")]
        public async Task<ActionResult<IEnumerable<LearningEquipment>>> GetLearningEquipmentByRatings([FromQuery] decimal[] ratings)
        {
            if (ratings == null || ratings.Length == 0)
            {
                return BadRequest("يرجى تقديم تقييم واحد على الأقل.");
            }

            var filteredHandmadeProducts = await _context.HandmadeProducts
                .Where(e => ratings.Contains(e.Rating ?? 0)) // تأكد من التعامل مع القيم null في Rating
                .ToListAsync();

            return Ok(filteredHandmadeProducts);
        }
        // GET: api/LearningEquipment/prices?minPrice={minPrice}&maxPrice={maxPrice}
        [HttpGet("prices")]
        public async Task<ActionResult<IEnumerable<HandmadeProduct>>> GetHandmadeProductsByPriceRange(decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.HandmadeProducts.AsQueryable();

            if (minPrice.HasValue)
            {
                query = query.Where(e => e.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(e => e.Price <= maxPrice.Value);
            }

            var filteredHandmadeProducts = await query.ToListAsync();
            return Ok(filteredHandmadeProducts);
        }


    }
}
