using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_Master.Models;
using backend_Master.DTO;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardItemHandmadeProductController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CardItemHandmadeProductController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/CardItemHandmadeProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardItemHandmadeProduct>>> GetCardItemHandmadeProducts()
        {
            return await _context.CardItemHandmadeProducts.ToListAsync();
        }

        // GET: api/CardItemHandmadeProduct/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CardItemHandmadeProduct>> GetCardItemHandmadeProduct(int id)
        {
            var cardItemHandmadeProduct = await _context.CardItemHandmadeProducts.FindAsync(id);

            if (cardItemHandmadeProduct == null)
            {
                return NotFound();
            }

            return cardItemHandmadeProduct;
        }
        [HttpGet("card/{cardId}")]
        public async Task<ActionResult<IEnumerable<CardItemHandmadeProduct>>> GetCardItemsByCardId(int cardId)
        {
            var cardItems = await _context.CardItemHandmadeProducts
                                          .Where(item => item.CardId == cardId)
                                          .ToListAsync();

            if (cardItems == null || cardItems.Count == 0)
            {
                return NotFound();
            }

            return Ok(cardItems);
        }


        // POST: api/CardItemHandmadeProduct
        //[HttpPost]
        //public async Task<ActionResult<CardItemHandmadeProduct>> PostCardItemHandmadeProduct([FromBody] CardItemHandmadeProductDto dto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // تحويل الـ DTO إلى كيان
        //    var cardItemHandmadeProduct = new CardItemHandmadeProduct
        //    {
        //        CardId = dto.CardId,
        //        ProductId = dto.ProductId,
        //        Quantity = dto.Quantity,
        //        Price = dto.Price,
        //        AddedAt = dto.AddedAt
        //    };

        //    _context.CardItemHandmadeProducts.Add(cardItemHandmadeProduct);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetCardItemHandmadeProduct), new { id = cardItemHandmadeProduct.CardItemId }, cardItemHandmadeProduct);
        //}
        [HttpPost]
        public async Task<ActionResult<CardItemHandmadeProduct>> PostCardItemHandmadeProduct([FromBody] CardItemHandmadeProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // التحقق مما إذا كان العنصر موجودًا مسبقًا
            var existingItem = await _context.CardItemHandmadeProducts
                .FirstOrDefaultAsync(ci => ci.CardId == dto.CardId && ci.ProductId == dto.ProductId);

            if (existingItem != null)
            {
                // إذا كان العنصر موجودًا، قم بزيادة الكمية
                existingItem.Quantity += dto.Quantity;
                _context.Entry(existingItem).State = EntityState.Modified;
            }
            else
            {
                // إذا لم يكن العنصر موجودًا، قم بإضافة عنصر جديد
                var cardItemHandmadeProduct = new CardItemHandmadeProduct
                {
                    CardId = dto.CardId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    Price = dto.Price,
                    AddedAt = dto.AddedAt
                };

                _context.CardItemHandmadeProducts.Add(cardItemHandmadeProduct);
            }

            // حفظ التغييرات في قاعدة البيانات
            await _context.SaveChangesAsync();

            // إرجاع رسالة نجاح
            return Ok(new { message = "Product added to cart successfully" });
        }


        // PUT: api/CardItemHandmadeProduct/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCardItemHandmadeProduct(int id, [FromBody] CardItemHandmadeProduct cardItemHandmadeProduct)
        {
            if (id != cardItemHandmadeProduct.CardItemId)
            {
                return BadRequest();
            }

            _context.Entry(cardItemHandmadeProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardItemHandmadeProductExists(id))
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

        // DELETE: api/CardItemHandmadeProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardItemHandmadeProduct(int id)
        {
            var cardItemHandmadeProduct = await _context.CardItemHandmadeProducts.FindAsync(id);
            if (cardItemHandmadeProduct == null)
            {
                return NotFound();
            }

            _context.CardItemHandmadeProducts.Remove(cardItemHandmadeProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CardItemHandmadeProductExists(int id)
        {
            return _context.CardItemHandmadeProducts.Any(e => e.CardItemId == id);
        }
    }
}
