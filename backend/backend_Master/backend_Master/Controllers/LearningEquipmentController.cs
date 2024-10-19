using backend_Master.DTO;
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
            var equipments = await _context.LearningEquipments.ToListAsync();
            if (equipments == null || equipments.Count == 0)
            {
                return NotFound(); // Return 404 if no data found
            }
            return Ok(equipments); // Return the list of equipments
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
        [HttpPost]
        public async Task<IActionResult> CreateLearningEquipment([FromForm] LearningEquipmentDto dto)
        {
            // التحقق من صلاحية النموذج
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var equipment = new LearningEquipment
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                CourseId = dto.CourseId,
                CreatedAt = DateTime.UtcNow
            };

            // مسار تخزين الصور
            string imageDirectory = @"C:\Users\Orange\Desktop\Master_Project\backend\image";
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            // حفظ الصور إذا كانت موجودة وتخزين اسم الملف فقط في قاعدة البيانات
            if (dto.Image1 != null)
            {
                var imageFileName1 = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image1.FileName);
                var imagePath1 = Path.Combine(imageDirectory, imageFileName1);
                using (var stream = new FileStream(imagePath1, FileMode.Create))
                {
                    await dto.Image1.CopyToAsync(stream);
                }
                equipment.ImageUrl1 = imageFileName1; // حفظ اسم الصورة فقط
            }

            if (dto.Image2 != null)
            {
                var imageFileName2 = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image2.FileName);
                var imagePath2 = Path.Combine(imageDirectory, imageFileName2);
                using (var stream = new FileStream(imagePath2, FileMode.Create))
                {
                    await dto.Image2.CopyToAsync(stream);
                }
                equipment.ImageUrl2 = imageFileName2; // حفظ اسم الصورة فقط
            }

            if (dto.Image3 != null)
            {
                var imageFileName3 = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image3.FileName);
                var imagePath3 = Path.Combine(imageDirectory, imageFileName3);
                using (var stream = new FileStream(imagePath3, FileMode.Create))
                {
                    await dto.Image3.CopyToAsync(stream);
                }
                equipment.ImageUrl3 = imageFileName3; // حفظ اسم الصورة فقط
            }

            // حفظ المعلومات في قاعدة البيانات
            _context.LearningEquipments.Add(equipment);
            await _context.SaveChangesAsync();

            return Ok(equipment);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLearningEquipment(int id, [FromForm] LearningEquipmentDto dto)
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var equipment = await _context.LearningEquipments.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }

            // Update equipment details
            equipment.Name = dto.Name;
            equipment.Description = dto.Description;
            equipment.Price = dto.Price;
            equipment.CategoryId = dto.CategoryId;
            equipment.CourseId = dto.CourseId;

            // Directory for images
            string imageDirectory = @"C:\Users\Orange\Desktop\Master_Project\backend\image";
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            // Update image 1 if a new image is uploaded
            if (dto.Image1 != null)
            {
                var newImage1 = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image1.FileName)}";
                var newPath1 = Path.Combine(imageDirectory, newImage1);
                using (var stream = new FileStream(newPath1, FileMode.Create))
                {
                    await dto.Image1.CopyToAsync(stream);
                }
                equipment.ImageUrl1 = newImage1;  // Update the image URL only if a new image is uploaded
            }

            // Update image 2 if a new image is uploaded
            if (dto.Image2 != null)
            {
                var newImage2 = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image2.FileName)}";
                var newPath2 = Path.Combine(imageDirectory, newImage2);
                using (var stream = new FileStream(newPath2, FileMode.Create))
                {
                    await dto.Image2.CopyToAsync(stream);
                }
                equipment.ImageUrl2 = newImage2;  // Update the image URL only if a new image is uploaded
            }

            // Update image 3 if a new image is uploaded
            if (dto.Image3 != null)
            {
                var newImage3 = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image3.FileName)}";
                var newPath3 = Path.Combine(imageDirectory, newImage3);
                using (var stream = new FileStream(newPath3, FileMode.Create))
                {
                    await dto.Image3.CopyToAsync(stream);
                }
                equipment.ImageUrl3 = newImage3;  // Update the image URL only if a new image is uploaded
            }

            // Save updated equipment
            _context.LearningEquipments.Update(equipment);
            await _context.SaveChangesAsync();

            return Ok();
        }




        private bool EquipmentExists(int id)
        {
            return _context.LearningEquipments.Any(e => e.EquipmentId == id);
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
