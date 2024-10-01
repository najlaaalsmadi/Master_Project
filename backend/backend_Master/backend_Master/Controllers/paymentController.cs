//using backend_Master.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using PayPalCheckoutSdk.Core;
//using PayPalCheckoutSdk.Orders;
//using PayPalHttp;
//using System.Security.Claims;

//namespace backend_Master.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PaymentController : ControllerBase
//    {
//        private PayPalEnvironment _environment;
//        private PayPalHttpClient _client;

//        public PaymentController()
//        {
//            _environment = new SandboxEnvironment("Your-Client-Id", "Your-Secret-Key");
//            _client = new PayPalHttpClient(_environment);
//        }

//        // إنشاء طلب الدفع
//        public async Task<IActionResult> CreatePayment()
//        {
//            var orderRequest = new OrderRequest()
//            {
//                CheckoutPaymentIntent = "CAPTURE",
//                PurchaseUnits = new List<PurchaseUnitRequest>()
//                {
//                    new PurchaseUnitRequest()
//                    {
//                        AmountWithBreakdown = new AmountWithBreakdown()
//                        {
//                            CurrencyCode = "USD",
//                            Value = "100.00"
//                        }
//                    }
//                },
//                ApplicationContext = new ApplicationContext()
//                {
//                    ReturnUrl = "https://example.com/success",
//                    CancelUrl = "https://example.com/cancel"
//                }
//            };

//            var request = new OrdersCreateRequest();
//            request.Prefer("return=representation");
//            request.RequestBody(orderRequest);

//            var response = await _client.Execute(request);
//            var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

//            var approvalLink = result.Links.FirstOrDefault(link => link.Rel == "approve")?.Href;

//            // إعادة توجيه المستخدم إلى رابط الدفع
//            return Redirect(approvalLink);
//        }

//        // تأكيد الدفع
//        public async Task<IActionResult> ConfirmPayment(string token)
//        {
//            var request = new OrdersCaptureRequest(token);
//            request.RequestBody(new OrderActionRequest());

//            var response = await _client.Execute(request);
//            var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

//            if (result.Status == "COMPLETED")
//            {
//                // حفظ تفاصيل الدفع في قاعدة البيانات
//                await SavePaymentDetails(result);
//                return RedirectToAction("Success");
//            }

//            return RedirectToAction("Failure");
//        }

//        // حفظ تفاصيل الدفع في قاعدة البيانات
//        private async Task SavePaymentDetails(PayPalCheckoutSdk.Orders.Order result)
//        {
//            using (var context = new MyDbContext())
//            {
//                int userId;
//                if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userId))
//                {
//                    PayPalCheckoutSdk.Orders.Order result1 = result;
//                    var payment = new Payment
//                    {
//                        PaymentId = int(result1.Id),
//                        Amount = Convert.ToDecimal(result1.PurchaseUnits[0].AmountWithBreakdown.Value),
//                        Status = result1.Status,
//                        PaymentDate = DateTime.Now,
//                        UserId = userId
//                    };

//                    context.Payments.Add(payment);
//                    await context.SaveChangesAsync();
//                }
//                else
//                {
//                    // في حال فشل التحويل، قم بمعالجة الخطأ كما تراه مناسبًا
//                    throw new Exception("Invalid user ID");
//                }
//            }
//        }
//    }
//}
