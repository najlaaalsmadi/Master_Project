using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_Master.Models; // Adjust namespace as needed
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_Master.DTO;
using System;

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

        // GET: api/CardItemLearningEquipment/card/1
        [HttpGet("card/{cardId}")]
        public async Task<ActionResult<IEnumerable<CardItemLearningEquipment>>> GetCardItemsByCardId(int cardId)
        {
            var cardItems = await _context.CardItemLearningEquipments
                                          .Where(item => item.CardId == cardId)
                                          .ToListAsync();

            if (cardItems == null || cardItems.Count == 0)
            {
                return NotFound(new { message = "لا توجد عناصر في السلة لهذا الكارت" });
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
                return NotFound(new { message = "العنصر غير موجود" });
            }

            return Ok(cardItem);
        }

        // POST: api/CardItemLearningEquipment
        [HttpPost]
        public async Task<ActionResult> PostCardItemLearningEquipment([FromBody] CardItemLearningEquipmentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // التحقق مما إذا كان العنصر موجودًا مسبقًا في السلة
            var existingItem = await _context.CardItemLearningEquipments
                .FirstOrDefaultAsync(ci => ci.CardId == dto.CardId && ci.EquipmentId == dto.EquipmentId);

            if (existingItem != null)
            {
                // إذا كان العنصر موجودًا، قم بزيادة الكمية
                existingItem.Quantity += dto.Quantity;
                existingItem.Price = dto.Price * existingItem.Quantity; // تحديث السعر بناءً على الكمية الجديدة
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
                    Price = dto.Price * dto.Quantity, // السعر يساوي السعر الأساسي × الكمية
                    AddedAt = dto.AddedAt ?? DateTime.UtcNow // تعيين الوقت الحالي إذا لم يكن محددًا
                };

                _context.CardItemLearningEquipments.Add(cardItem);
            }

            // حفظ التغييرات في قاعدة البيانات
            await _context.SaveChangesAsync();

            return Ok(new { message = "تم إضافة العنصر إلى السلة بنجاح" });
        }

        // PUT: api/CardItemLearningEquipment/5
        // PUT: api/CardItemLearningEquipment/5
        // PUT: api/CardItemLearningEquipment/5
        [HttpPut("{EquipmentId}")]
        public async Task<IActionResult> PutCardItemLearningEquipment(int EquipmentId, [FromBody] UpdateQuantityDto updateQuantityDto)
        {
            // البحث عن العنصر في قاعدة البيانات باستخدام المعرّف
            var cardItem = await _context.CardItemLearningEquipments
                                              .Where(item => item.EquipmentId == EquipmentId)
                                              .FirstOrDefaultAsync();

            if (cardItem == null)
            {
                return NotFound(new { message = "العنصر غير موجود" });
            }

            // تعديل الكمية فقط
            cardItem.Quantity = updateQuantityDto.Quantity;

            // وضع الحالة على "Modified" لإخبار الـ DbContext بأن هذا الكيان تم تعديله
            _context.Entry(cardItem).State = EntityState.Modified;

            try
            {
                // حفظ التغييرات في قاعدة البيانات
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // في حالة وجود خطأ متعلق بتزامن البيانات
                if (!CardItemLearningEquipmentExists(EquipmentId))
                {
                    return NotFound(new { message = "العنصر غير موجود عند التحديث" });
                }
                else
                {
                    throw;
                }
            }

            // إرجاع حالة "NoContent" للدلالة على نجاح العملية دون أي بيانات إضافية
            return NoContent();
        }

        // دالة تحقق من وجود العنصر
        private bool CardItemLearningEquipmentExists(int id)
        {
            return _context.CardItemLearningEquipments.Any(e => e.EquipmentId == id);
        }

       

        // DELETE: api/CardItemLearningEquipment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardItemLearningEquipment(int id)
        {
            var cardItem = await _context.CardItemLearningEquipments.FirstOrDefaultAsync(i => i.EquipmentId == id);
            if (cardItem == null)
            {
                return NotFound(new { message = "العنصر غير موجود" });
            }

            _context.CardItemLearningEquipments.Remove(cardItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

       
    }
}
