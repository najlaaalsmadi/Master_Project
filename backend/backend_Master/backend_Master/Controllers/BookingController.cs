using backend_Master.DTO;
using backend_Master.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {


        private readonly MyDbContext _context;

        public BookingController(MyDbContext context)
            {
                _context = context;
            }

            // GET: api/bookings
            [HttpGet]
            public IActionResult GetAllBookings()
            {
                var bookings = _context.Bookings.Take(1000).ToList();
                return Ok(bookings);
            }

            // GET: api/bookings/{id}
            [HttpGet("{id}")]
            public IActionResult GetBookingById(int id)
            {
                var booking = _context.Bookings.Find(id);

                if (booking == null)
                {
                    return NotFound();
                }

                return Ok(booking);
            }

        // POST: api/bookings
        // POST: api/Booking
        [HttpPost]
        [Route("api/Booking")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDto)
        {
            if (ModelState.IsValid)
            {
                var booking = new Booking
                {
                    StudentId = bookingDto.StudentId,
                    EventId = bookingDto.EventId,
                    BookingDate = bookingDto.BookingDate,
                    Status = bookingDto.Status
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
                return Ok(booking);
            }
            return BadRequest(ModelState);
        }





        // PUT: api/bookings/{id}
        [HttpPut("{id}")]
            public IActionResult UpdateBooking(int id, [FromBody] Booking updatedBooking)
            {
                if (id != updatedBooking.BookingId)
                {
                    return BadRequest();
                }

                var existingBooking = _context.Bookings.Find(id);
                if (existingBooking == null)
                {
                    return NotFound();
                }

                existingBooking.StudentId = updatedBooking.StudentId;
                existingBooking.EventId = updatedBooking.EventId;
                existingBooking.BookingDate = updatedBooking.BookingDate;
                existingBooking.Status = updatedBooking.Status;

                _context.Bookings.Update(existingBooking);
                _context.SaveChanges();

                return NoContent();
            }

            // DELETE: api/bookings/{id}
            [HttpDelete("{id}")]
            public IActionResult DeleteBooking(int id)
            {
                var booking = _context.Bookings.Find(id);
                if (booking == null)
                {
                    return NotFound();
                }

                _context.Bookings.Remove(booking);
                _context.SaveChanges();

                return NoContent();
            }
        }

    }

