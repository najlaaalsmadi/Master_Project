using backend_Master.DTO;
using backend_Master.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardItemController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CardItemController(MyDbContext context)
        {
            _context = context;
        }

        //// Check if a product is already in the cart
        //private bool IsProductInCart(int cardId, int productId)
        //{
        //    return _context.CardItems.Any(ci => ci.CardId == cardId && ci.ProductId == productId);
        //}

        //// Add or update a cart item
        //private CardItem AddOrUpdateCardItem(CardItemDTO cardItemDto)
        //{
        //    // Check if the card exists
        //    var card = _context.Cards.SingleOrDefault(c => c.UserId == cardItemDto.CardId);
        //    if (card == null)
        //    {
        //        throw new Exception($"Card with ID {cardItemDto.CardId} does not exist.");
        //    }

        //    // Check if the product is already in the cart
        //    var existingCardItem = _context.CardItems
        //        .FirstOrDefault(ci => ci.CardId == cardItemDto.CardId && ci.ProductId == cardItemDto.ProductId);

        //    if (existingCardItem != null)
        //    {
        //        // Update quantity if the product already exists in the cart
        //        existingCardItem.Quantity += cardItemDto.Quantity;
        //    }
        //    else
        //    {
        //        // Create a new cart item if it doesn't exist
        //        existingCardItem = new CardItem
        //        {
        //            CardId = cardItemDto.CardId,
        //            ProductId = cardItemDto.ProductId,
        //            Quantity = cardItemDto.Quantity,
        //            Price = cardItemDto.Price,
        //            AddedAt = DateTime.Now
        //        };
        //        _context.CardItems.Add(existingCardItem);
        //    }

        //    _context.SaveChanges();
        //    return existingCardItem;
        //}

        //// Add a product to the cart
        //[HttpPost]
        //public IActionResult AddToCart([FromBody] CardItemDTO cardItemDto)
        //{
        //    if (cardItemDto == null)
        //    {
        //        return BadRequest("Invalid data.");
        //    }

        //    try
        //    {
        //        var addedOrUpdatedCardItem = AddOrUpdateCardItem(cardItemDto);

        //        return Ok(new
        //        {
        //            message = "Product added to cart successfully.",
        //            cardItem = addedOrUpdatedCardItem
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message, details = ex.InnerException?.Message });
        //    }
        //}
        private bool IsProductInCart(int cardId, int productId)
        {
            return _context.CardItems.Any(ci => ci.CardId == cardId && ci.ProductId == productId);
        }

        private bool IsEquipmentInCart(int cardId, int equipmentId)
        {
            return _context.CardItems.Any(ci => ci.CardId == cardId && ci.EquipmentId == equipmentId);
        }

        // Add or update a cart item
        private CardItem AddOrUpdateCardItem(CardItemDTO cardItemDto)
        {
            // Verify the card exists
            var card = _context.Cards.SingleOrDefault(c => c.CardId == cardItemDto.CardId);
            if (card == null)
            {
                throw new Exception($"Card with ID {cardItemDto.CardId} does not exist.");
            }

            CardItem existingCardItem = null;

            // Check if the item is a product or equipment and handle accordingly
            if (cardItemDto.ProductId != null)
            {
                // Check if the product already exists in the cart
                existingCardItem = _context.CardItems
                    .FirstOrDefault(ci => ci.CardId == cardItemDto.CardId && ci.ProductId == cardItemDto.ProductId);
            }
            else if (cardItemDto.EquipmentId != null)
            {
                // Check if the equipment already exists in the cart
                existingCardItem = _context.CardItems
                    .FirstOrDefault(ci => ci.CardId == cardItemDto.CardId && ci.EquipmentId == cardItemDto.EquipmentId);
            }

            if (existingCardItem != null)
            {
                // Update quantity if the item is already in the cart
                existingCardItem.Quantity += cardItemDto.Quantity ?? 1; // Set to 1 if null
            }
            else
            {
                // Create a new cart item if it does not exist
                existingCardItem = new CardItem
                {
                    CardId = cardItemDto.CardId,
                    ProductId = cardItemDto.ProductId,
                    EquipmentId = cardItemDto.EquipmentId,
                    Quantity = cardItemDto.Quantity ?? 1, // Set to 1 if null
                    Price = cardItemDto.Price ?? 0, // Set to 0 if null
                    AddedAt = DateTime.Now
                };
                _context.CardItems.Add(existingCardItem);
            }

            _context.SaveChanges();
            return existingCardItem;
        }

        // Add a product to the cart
        [HttpPost]
        public IActionResult AddToCart([FromBody] CardItemDTO cardItemDto)
        {
            if (cardItemDto == null || cardItemDto.CardId == null ||
                (cardItemDto.ProductId == null && cardItemDto.EquipmentId == null))
            {
                return BadRequest("Invalid data. Please provide valid card and product or equipment IDs.");
            }

            try
            {
                var addedOrUpdatedCardItem = AddOrUpdateCardItem(cardItemDto);

                return Ok(new
                {
                    message = "Item added to cart successfully.",
                    cardItem = addedOrUpdatedCardItem
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, details = ex.InnerException?.Message });
            }
        }


        // Get all items in the cart by CardId
        [HttpGet("cart-items/{cardId}")]
        public IActionResult GetCartItems(int cardId)
        {
            var cartItems = _context.CardItems.Where(ci => ci.CardId == cardId).ToList();

            if (!cartItems.Any())
            {
                return NotFound("No items found in the cart.");
            }

            return Ok(cartItems);
        }

        // Remove an item from the cart
        [HttpDelete("remove-item/{cardId}/{productId}")]
        public IActionResult RemoveCartItem(int cardId, int productId)
        {
            var cartItem = _context.CardItems
                .FirstOrDefault(ci => ci.CardId == cardId && ci.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound("Item not found in the cart.");
            }

            _context.CardItems.Remove(cartItem);
            _context.SaveChanges();

            return Ok(new { message = "Product removed from the cart." });
        }
        // الحصول على عنصر معين من السلة بواسطة ID
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCardItemsByUserId(int userId)
        {
            var cardItems = await _context.CardItems
                                          .Where(ci => ci.Card.UserId == userId) // افترض أن CardId هو UserId
                                          .ToListAsync();
            if (cardItems == null || cardItems.Count == 0)
            {
                return NotFound();
            }
            return Ok(cardItems);
        }


        // الحصول على جميع العناصر في السلة بواسطة CardId
        [HttpGet("card/{cardId}")]
        public IActionResult GetCardItems(int cardId)
        {
            var items = _context.CardItems.Where(ci => ci.CardId == cardId).ToList();
            return Ok(items);
        }
    }
}
