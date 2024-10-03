using backend_Master.DTO;
using backend_Master.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentRepositoryController : ControllerBase
    {
        private readonly MyDbContext _context;

        public PaymentRepositoryController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost("Payments")]
        public IActionResult CreatePayment([FromBody] PaymentsCourseDto request)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Add payment details to the database
                var payment = new PaymentsCourse
                {
                    UserId = request.UserId,
                    CourseId = request.CourseId,
                    PaymentMethod = request.PaymentMethod,
                    Amount = request.Amount,
                    PaymentDate = DateTime.Now,
                    PayerId = GeneratePayerId(), // Generate unique payer ID
                    Status = "Paid",
                    CardName = request.CardName,
                    TransactionId = GenerateTransactionId(),
                     FirstName = request.FirstName,
                    LastName = request.LastName,
                    AddressLine1 = request.AddressLine1,
                    AddressLine2 = request.AddressLine2,
                    City = request.City,
                    State = request.State,
                    ZipCode = request.ZipCode,
                    Country = request.Country,
                    CountryCallingCode = request.CountryCallingCode,
                    PhoneNumber = request.PhoneNumber
                };

                _context.PaymentsCourses.Add(payment);
                _context.SaveChanges();

                return Ok(new { message = "Payment successful." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GeneratePayerId()
        {
            // Generate unique payer ID logic here
            return Guid.NewGuid().ToString(); // Example
        }

        private string GenerateTransactionId()
        {
            // Generate unique transaction ID logic here
            return Guid.NewGuid().ToString(); // Example
        }
    }
}
