using backend_Master.DTO;
using backend_Master.Models; // نموذج Card
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // للتأكد من وجود Include
using PayPalCheckoutSdk.Orders;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CardController(MyDbContext context)
        {
            _context = context;
        }

       
        // إضافة سلة جديدة
        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] Models.Card card)
        {
            // تعيين CardId ليكون نفس UserId
            card.CardId = (int)card.UserId; // تأكد من وجود UserId في الـ card

            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCard), new { id = card.CardId }, card);
        }

        // الحصول على سلة بواسطة ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCard(int id)
        {
            var card = await _context.Cards.Include(c => c.CardItems)
                                            .FirstOrDefaultAsync(c => c.CardId == id);
            if (card == null) return NotFound();
            return Ok(card);
        }
    }
}
