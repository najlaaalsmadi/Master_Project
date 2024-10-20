using backend_Master.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using Stripe;
using Stripe.Climate;
using backend_Master.DTO;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    //private readonly MyDbContext _context;

    //public PaymentController(MyDbContext context)
    //{
    //    _context = context;
    //}
    //[HttpPost("MakePayment")]
    //public async Task<IActionResult> MakePayment([FromBody] PaymentDto paymentDto)
    //{
    //    if (paymentDto == null)
    //    {
    //        return BadRequest("Payment information is required.");
    //    }

    //    using var transaction = await _context.Database.BeginTransactionAsync();

    //    try
    //    {
    //        // Step 1: Get the user's card items (Handmade Products and Learning Equipment)
    //        var handmadeItems = await _context.CardItemHandmadeProducts
    //            .Where(ci => ci.CardId == paymentDto.UserId)
    //            .ToListAsync();

    //        var learningItems = await _context.CardItemLearningEquipments
    //            .Where(ci => ci.CardId == paymentDto.UserId)
    //            .ToListAsync();

    //        // Step 2: Calculate the total price
    //        decimal totalPrice = (decimal)(handmadeItems.Sum(item => item.Quantity * (item.Price ?? 0)) +
    //                             learningItems.Sum(item => item.Quantity * (item.Price ?? 0)));

    //        // Step 3: Create the Order in the Orders table
    //        var order = new Order
    //        {
    //            UserId = paymentDto.UserId,
    //            TotalAmount = totalPrice,
    //            Status = "completed",
    //            CreatedAt = DateTime.UtcNow // استخدم UTC للتواريخ
    //        };
    //        await _context.Orders.AddAsync(order);
    //        await _context.SaveChangesAsync();  // Save order to get the orderId

    //        // Step 4: Add order items to the OrderItems table
    //        var orderItems = new List<OrderItem>();
    //        orderItems.AddRange(handmadeItems.Select(item => new OrderItem
    //        {
    //            OrderId = order.OrderId,
    //            CardItemId1 = item.CardItemId,
    //            Quantity = item.Quantity,
    //            Price = item.Price ?? 0
    //        }));

    //        orderItems.AddRange(learningItems.Select(item => new OrderItem
    //        {
    //            OrderId = order.OrderId,
    //            CardItemId2 = item.CardItemId,
    //            Quantity = item.Quantity,
    //            Price = item.Price ?? 0
    //        }));

    //        await _context.OrderItems.AddRangeAsync(orderItems);
    //        await _context.SaveChangesAsync();

    //        // Step 5: Create the Payment record
    //        var payment = new Payment
    //        {
    //            Amount = totalPrice,
    //            PaymentMethod = paymentDto.PaymentMethod,
    //            UserId = paymentDto.UserId,
    //            OrderId = order.OrderId,
    //            FirstName = paymentDto.FirstName,
    //            LastName = paymentDto.LastName,
    //            AddressLine1 = paymentDto.AddressLine1,
    //            AddressLine2 = paymentDto.AddressLine2,
    //            City = paymentDto.City,
    //            State = paymentDto.State,
    //            ZipCode = paymentDto.ZipCode,
    //            Country = paymentDto.Country,
    //            CountryCallingCode = paymentDto.CountryCallingCode,
    //            PhoneNumber = paymentDto.PhoneNumber,
    //            CreatedAt = DateTime.UtcNow
    //        };

    //        await _context.Payments.AddAsync(payment);
    //        await _context.SaveChangesAsync();

    //        // Step 6: Delete items from CardItem tables
    //        _context.CardItemHandmadeProducts.RemoveRange(handmadeItems);
    //        _context.CardItemLearningEquipments.RemoveRange(learningItems);
    //        await _context.SaveChangesAsync();

    //        await transaction.CommitAsync();

    //        return Ok(new { success = true, orderId = order.OrderId });
    //    }
    //    catch (DbUpdateException dbEx)
    //    {
    //        // تسجيل تفاصيل الأخطاء في قاعدة البيانات
    //        var errorMessage = $"Database update failed: {dbEx.Message}. ";
    //        if (dbEx.InnerException != null)
    //        {
    //            errorMessage += $"Inner Exception: {dbEx.InnerException.Message}.";
    //        }
    //        Console.WriteLine(errorMessage);
    //        await transaction.RollbackAsync();
    //        return BadRequest("Database error occurred. Please try again later.");
    //    }
    //    catch (Exception ex)
    //    {
    //        // تسجيل تفاصيل الأخطاء العامة
    //        var errorMessage = $"Transaction failed: {ex.Message}. ";
    //        if (ex.InnerException != null)
    //        {
    //            errorMessage += $"Inner Exception: {ex.InnerException.Message}.";
    //        }
    //        Console.WriteLine(errorMessage);
    //        await transaction.RollbackAsync();
    //        return BadRequest("An error occurred during the payment process.");
    //    }
    //}


    private readonly MyDbContext _context;
    private readonly string _secretKey;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(MyDbContext context, IConfiguration configuration, ILogger<PaymentController> logger)
    {
        _context = context;
        _secretKey = configuration["Stripe:SecretKey"];
        StripeConfiguration.ApiKey = _secretKey;
        _logger = logger;
    }

    [HttpPost("create-checkout-session")]
    public async Task<IActionResult> CreateCheckoutSession([FromBody] PaymentDto paymentDto)
    {
        if (paymentDto == null || paymentDto.UserId <= 0)
        {
            return BadRequest("Payment information is required.");
        }

        // سجل الطلب
        _logger.LogInformation($"Creating checkout session for user {paymentDto.UserId}");

        // خطوة 1: الحصول على عناصر السلة للمستخدم
        var handmadeItems = await _context.CardItemHandmadeProducts
            .Where(ci => ci.CardId == paymentDto.UserId)
            .ToListAsync();

        var learningItems = await _context.CardItemLearningEquipments
            .Where(ci => ci.CardId == paymentDto.UserId)
            .ToListAsync();

        if (!handmadeItems.Any() && !learningItems.Any())
        {
            return BadRequest("No items in the cart.");
        }

        // خطوة 2: إنشاء قائمة العناصر للمدفوعات
        var lineItems = new List<SessionLineItemOptions>();
        var orderItems = new List<OrderItem>();

        // إضافة العناصر اليدوية إلى قائمة الدفع
        foreach (var item in handmadeItems)
        {
            var product = await _context.HandmadeProducts.SingleOrDefaultAsync(a => a.ProductId == item.ProductId);
            if (product == null) continue;

            lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(item.Price * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Name,
                    },
                },
                Quantity = item.Quantity,
            });

            orderItems.Add(new OrderItem
            {
                CardItemId1 = item.CardItemId, // استخدام CardItemId الصحيح
                ProductId = item.ProductId,    // إضافة ProductId
                Quantity = item.Quantity,
                Price = item.Price ?? 0
            });
        }

        // إضافة عناصر معدات التعلم إلى قائمة الدفع
        foreach (var item in learningItems)
        {
            var product = await _context.LearningEquipments.SingleOrDefaultAsync(a => a.EquipmentId == item.EquipmentId);
            if (product == null) continue;

            lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(item.Price * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Name,
                    },
                },
                Quantity = item.Quantity,
            });

            orderItems.Add(new OrderItem
            {
                CardItemId2 = item.CardItemId,  // استخدام CardItemId الصحيح
                EquipmentId = item.EquipmentId, // إضافة EquipmentId
                Quantity = item.Quantity,
                Price = item.Price ?? 0
            });
        }

        // حساب المبلغ الإجمالي
        long totalAmountInCents = (long)lineItems.Sum(item => item.Quantity * item.PriceData.UnitAmount);

        // خطوة 3: إعداد خيارات جلسة Stripe
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = "http://127.0.0.1:5500/frontend/user/Index.html",
            CancelUrl = "http://127.0.0.1:5500/frontend/user/equipmentshop.html",
        };

        Session session;
        try
        {
            var service = new SessionService();
            session = await service.CreateAsync(options);
        }
        catch (StripeException ex)
        {
            _logger.LogError($"Stripe error: {ex.Message}");
            return BadRequest("Failed to create Stripe session.");
        }

        // خطوة 4: إنشاء طلب في قاعدة البيانات
        var order = new backend_Master.Models.Order
        {
            UserId = paymentDto.UserId,
            TotalAmount = totalAmountInCents / 100,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        // خطوة 5: إضافة عناصر الطلب إلى قاعدة البيانات
        foreach (var orderItem in orderItems)
        {
            orderItem.OrderId = order.OrderId;
            await _context.OrderItems.AddAsync(orderItem);
        }
        await _context.SaveChangesAsync();

        // خطوة 6: إزالة العناصر من السلة بعد الدفع الناجح
        _context.CardItemHandmadeProducts.RemoveRange(handmadeItems);
        _context.CardItemLearningEquipments.RemoveRange(learningItems);
        await _context.SaveChangesAsync();

        // خطوة 7: إضافة معلومات الدفع إلى قاعدة البيانات
        var payment = new Payment
        {
            UserId = paymentDto.UserId,
            OrderId = order.OrderId,
            State = "completed",
            PaymentMethod = "Stripe",
            Amount = totalAmountInCents / 100,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        // خطوة 8: إرجاع معرف الجلسة لجلسة الدفع في Stripe
        return Ok(new { sessionId = session.Id });
    }
    //[HttpPost("create-checkout-session")]
    //public async Task<IActionResult> CreateCheckoutSession([FromBody] PaymentDto paymentDto)
    //{
    //    if (paymentDto == null)
    //    {
    //        return BadRequest("Payment information is required.");
    //    }

    //    // Step 1: Get the user's card items (Handmade Products and Learning Equipment)
    //    var handmadeItems = await _context.CardItemHandmadeProducts
    //        .Where(ci => ci.CardId == paymentDto.UserId)
    //        .ToListAsync();

    //    var learningItems = await _context.CardItemLearningEquipments
    //        .Where(ci => ci.CardId == paymentDto.UserId)
    //        .ToListAsync();

    //    // Step 2: إنشاء قائمة العناصر للدفع
    //    var lineItems = new List<SessionLineItemOptions>();
    //    var orderItems = new List<OrderItem>();

    //    // إضافة العناصر اليدوية إلى قائمة الدفع
    //    foreach (var item in handmadeItems)
    //    {
    //        var product = await _context.HandmadeProducts.SingleOrDefaultAsync(a => a.ProductId == item.ProductId);
    //        if (product == null) continue; // التعامل مع حالة عدم وجود المنتج

    //        lineItems.Add(new SessionLineItemOptions
    //        {
    //            PriceData = new SessionLineItemPriceDataOptions
    //            {
    //                UnitAmount = (long)(item.Price * 100),
    //                Currency = "usd",
    //                ProductData = new SessionLineItemPriceDataProductDataOptions
    //                {
    //                    Name = product.Name,
    //                },
    //            },
    //            Quantity = item.Quantity,
    //        });

    //        orderItems.Add(new OrderItem
    //        {
    //            CardItemId1 = item.CardItemId,
    //            Quantity = item.Quantity,
    //            Price = item.Price ?? 0
    //        });
    //    }

    //    // إضافة عناصر التعليم إلى قائمة الدفع
    //    foreach (var item in learningItems)
    //    {
    //        var product = await _context.LearningEquipments.SingleOrDefaultAsync(a => a.EquipmentId == item.EquipmentId);
    //        if (product == null) continue; // التعامل مع حالة عدم وجود المنتج

    //        lineItems.Add(new SessionLineItemOptions
    //        {
    //            PriceData = new SessionLineItemPriceDataOptions
    //            {
    //                UnitAmount = (long)(item.Price * 100),
    //                Currency = "usd",
    //                ProductData = new SessionLineItemPriceDataProductDataOptions
    //                {
    //                    Name = product.Name,
    //                },
    //            },
    //            Quantity = item.Quantity,
    //        });

    //        orderItems.Add(new OrderItem
    //        {
    //            CardItemId2 = item.CardItemId,
    //            Quantity = item.Quantity,
    //            Price = item.Price ?? 0
    //        });
    //    }

    //    // حساب المبلغ الإجمالي
    //    long totalAmountInCents = (long)lineItems.Sum(item => item.Quantity * item.PriceData.UnitAmount);

    //    // Step 3: إعداد خيارات الجلسة لـ Stripe
    //    var options = new SessionCreateOptions
    //    {
    //        PaymentMethodTypes = new List<string> { "card" },
    //        LineItems = lineItems,
    //        Mode = "payment",
    //        SuccessUrl = "http://127.0.0.1:5500/frontend/user/Index.html",
    //        CancelUrl = "http://127.0.0.1:5500/frontend/user/equipmentshop.html",
    //    };

    //    var service = new SessionService();
    //    Session session = await service.CreateAsync(options);

    //    // إنشاء الطلب في قاعدة البيانات
    //    var order = new backend_Master.Models.Order
    //    {
    //        UserId = paymentDto.UserId,
    //        TotalAmount = totalAmountInCents / 100,
    //        Status = "pending",
    //        CreatedAt = DateTime.UtcNow
    //    };
    //    await _context.Orders.AddAsync(order);
    //    await _context.SaveChangesAsync();

    //    // Step 4: إضافة عناصر الطلب إلى قاعدة البيانات
    //    foreach (var orderItem in orderItems)
    //    {
    //        orderItem.OrderId = order.OrderId;
    //        await _context.OrderItems.AddAsync(orderItem);
    //    }
    //    await _context.SaveChangesAsync();

    //    // Step 5: إرجاع معرف الجلسة
    //    return Ok(new { sessionId = session.Id });
    //}

}

