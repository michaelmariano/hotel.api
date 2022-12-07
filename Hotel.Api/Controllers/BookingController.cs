using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingApp _bookingApp;

        public BookingController(IBookingApp bookingApp)
        {
            _bookingApp = bookingApp;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await _bookingApp.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Booking booking)
        {
            return Created("/api/booking/{id}", await _bookingApp.InsertAsync(booking));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Booking booking)
        {
            await _bookingApp.UpdateAsync(booking);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _bookingApp.DeleteAsync(id);

            return Ok();
        }
    }
}
