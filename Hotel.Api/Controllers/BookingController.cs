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
        public async Task<Booking?> Get([FromRoute] int id)
        {
            return await _bookingApp.GetAsync(id);
        }

        [HttpPost]
        public async Task<int> Insert([FromBody] Booking booking)
        {
            return await _bookingApp.InsertAsync(booking);
        }

        [HttpPut]
        public async Task Update([FromBody] Booking booking)
        {
            await _bookingApp.UpdateAsync(booking);
        }

        [HttpDelete("{id}")]
        public async Task Delete([FromRoute] int id)
        {
            await _bookingApp.DeleteAsync(id);
        }
    }
}
