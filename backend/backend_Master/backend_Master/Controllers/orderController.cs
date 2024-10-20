using backend_Master.DTO;
using backend_Master.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static backend_Master.DTO.DTOOrder;

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

    [HttpGet("orders")]
    public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
    {
        // جلب الطلبات مع المستخدم والعناصر المرتبطة بها (المنتجات اليدوية والمعدات التعليمية)
        var orders = await _context.Orders
            .Include(o => o.User)  // جلب المستخدم المرتبط بالطلب
            .Include(o => o.OrderItems)  // جلب العناصر المرتبطة بكل طلب
                .ThenInclude(oi => oi.CardItemId1Navigation)  // جلب المنتجات اليدوية
                    .ThenInclude(p => p.Product)  // جلب تفاصيل المنتج اليدوي
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.CardItemId2Navigation)  // جلب المعدات التعليمية
                    .ThenInclude(e => e.Equipment)  // جلب تفاصيل المعدات التعليمية
            .ToListAsync();

        // تحويل الطلبات إلى OrderDTO
        var orderDTOs = orders.Select(o => new OrderDTO
        {
            OrderId = o.OrderId,
            UserName = o.User?.Name ?? "Unknown",  // عرض اسم المستخدم أو "غير معروف"
            TotalAmount = o.TotalAmount ?? 0,
            Status = o.Status ?? "Pending",
            CreatedAt = o.CreatedAt,
            // تحويل عناصر الطلب إلى OrderItemDTO
            OrderItems = o.OrderItems.Select(oi => new OrderItemDTO
            {
                OrderItemId = oi.OrderItemId,
                ProductName = oi.CardItemId1Navigation?.Product?.Name  // إذا كان العنصر منتج يدوي
                    ?? oi.CardItemId2Navigation?.Equipment?.Name  // إذا كان العنصر معدات تعليمية
                    ?? "Unknown",  // إذا لم يكن المنتج أو المعدات معروفين
                Quantity = oi.Quantity ?? 0,
                Price = oi.Price ?? 0
            }).ToList()  // قائمة بالعناصر المرتبطة بالطلب
        }).ToList();

        return Ok(orderDTOs);
    }


}
