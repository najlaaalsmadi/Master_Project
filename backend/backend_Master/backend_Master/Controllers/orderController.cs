using backend_Master.DTO;
using backend_Master.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly MyDbContext _context;

    public OrderController(MyDbContext context)
    {
        _context = context;
    }

    [HttpPost("PlaceOrder")]
    public async Task<IActionResult> PlaceOrder([FromBody] OrderDto orderDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Create the Order
            var order = new Order
            {
                UserId = orderDto.UserId,
                TotalAmount = orderDto.TotalAmount,
                Status = "pending"
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Create OrderItems from card items
            foreach (var item in orderDto.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    CardItemId1 = item.CardItemId1,
                    CardItemId2 = item.CardItemId2,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _context.OrderItems.Add(orderItem);

                // Remove items from CardItem_Handmade_Products and CardItem_Learning_Equipment
                var cardItemHandmade = await _context.CardItemHandmadeProducts.FindAsync(item.CardItemId1);
                if (cardItemHandmade != null) _context.CardItemHandmadeProducts.Remove(cardItemHandmade);

                var cardItemLearning = await _context.CardItemLearningEquipments.FindAsync(item.CardItemId2);
                if (cardItemLearning != null) _context.CardItemLearningEquipments.Remove(cardItemLearning);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return Ok(order);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, "Error processing your order.");
        }
    }
}
