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

        /// <summary>
        /// Returns a booking by id.
        /// </summary>
        /// <param name="id">Id of booking.</param>        
        [ProducesResponseType(typeof(Booking), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await _bookingApp.GetAsync(id));
        }

        /// <summary>
        /// Insert a booking.
        /// </summary>             
        [ProducesResponseType(typeof(int), 201)]
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Booking booking)
        {
            return Created("/api/booking/{id}", await _bookingApp.InsertAsync(booking));
        }

        /// <summary>
        /// Update a booking.
        /// </summary>             
        [ProducesResponseType(typeof(void), 200)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Booking booking)
        {
            await _bookingApp.UpdateAsync(booking);

            return Ok();
        }

        /// <summary>
        /// Delete a booking.
        /// </summary>        
        /// <param name="id">Id of booking.</param>      
        [ProducesResponseType(typeof(void), 200)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _bookingApp.DeleteAsync(id);

            return Ok();
        }
    }
}
