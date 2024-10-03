using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_Master.Models; // Adjust namespace as needed
using System.Collections.Generic;
using System.Threading.Tasks;
using backend_Master.DTO;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardItemLearningEquipmentController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CardItemLearningEquipmentController(MyDbContext context)
        {
            _context = context;
        }
      

        // GET: api/CardItemLearningEquipment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardItemLearningEquipment>>> GetCardItems()
        {
            return await _context.CardItemLearningEquipments.ToListAsync();
        }
        [HttpGet("card/{cardId}")]
        public async Task<ActionResult<IEnumerable<CardItemLearningEquipment>>> GetCardItemsByCardId(int cardId)
        {
            var cardItems = await _context.CardItemLearningEquipments
                                          .Where(item => item.CardId == cardId)
                                          .ToListAsync();

            if (cardItems == null || cardItems.Count == 0)
            {
                return NotFound();
            }

            return Ok(cardItems);
        }

        // GET: api/CardItemLearningEquipment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CardItemLearningEquipment>> GetCardItem(int id)
        {
            var cardItem = await _context.CardItemLearningEquipments.FindAsync(id);

            if (cardItem == null)
            {
                return NotFound();
            }

            return cardItem;
        }

        // POST: api/CardItemLearningEquipment
        // POST: api/CardItemLearningEquipment
        //[HttpPost]
        //public async Task<ActionResult<CardItemLearningEquipment>> PostCardItemLearningEquipment([FromBody] CardItemLearningEquipmentDTO dto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Map DTO to the entity model
        //    var cardItem = new CardItemLearningEquipment
        //    {
        //        CardId = dto.CardId,
        //        EquipmentId = dto.EquipmentId,
        //        Quantity = dto.Quantity,
        //        Price = dto.Price,
        //        AddedAt = dto.AddedAt ?? DateTime.UtcNow // Set to current time if not provided
        //    };

        //    _context.CardItemLearningEquipments.Add(cardItem);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetCardItem), new { id = cardItem.CardItemId }, cardItem);
        //}

        [HttpPost]
        public async Task<ActionResult<CardItemLearningEquipment>> PostCardItemLearningEquipment([FromBody] CardItemLearningEquipmentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // التحقق مما إذا كان العنصر موجودًا مسبقًا
            var existingItem = await _context.CardItemLearningEquipments
                .FirstOrDefaultAsync(ci => ci.CardId == dto.CardId && ci.EquipmentId == dto.EquipmentId);

            if (existingItem != null)
            {
                // إذا كان العنصر موجودًا، قم بزيادة الكمية
                existingItem.Quantity += dto.Quantity;
                _context.Entry(existingItem).State = EntityState.Modified;
            }
            else
            {
                // إذا لم يكن العنصر موجودًا، قم بإضافة عنصر جديد
                var cardItem = new CardItemLearningEquipment
                {
                    CardId = dto.CardId,
                    EquipmentId = dto.EquipmentId,
                    Quantity = dto.Quantity,
                    Price = dto.Price,
                    AddedAt = dto.AddedAt ?? DateTime.UtcNow // تعيين الوقت الحالي إذا لم يكن موجودًا
                };

                _context.CardItemLearningEquipments.Add(cardItem);
            }

            // حفظ التغييرات في قاعدة البيانات
            await _context.SaveChangesAsync();

            return Ok(new { message = "Item added to cart successfully" });
        }



        // PUT: api/CardItemLearningEquipment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCardItemLearningEquipment(int id, CardItemLearningEquipment dto)
        {
            if (id != dto.CardItemId)
            {
                return BadRequest();
            }

            _context.Entry(dto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardItemLearningEquipmentExists(id))
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

        // DELETE: api/CardItemLearningEquipment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardItemLearningEquipment(int id)
        {
            var cardItem = await _context.CardItemLearningEquipments.FindAsync(id);
            if (cardItem == null)
            {
                return NotFound();
            }

            _context.CardItemLearningEquipments.Remove(cardItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CardItemLearningEquipmentExists(int id)
        {
            return _context.CardItemLearningEquipments.Any(e => e.CardItemId == id);
        }
    }
}
