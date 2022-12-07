using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomApp _roomApp;

        public RoomController(IRoomApp roomApp)
        {
            _roomApp = roomApp;
        }

        /// <summary>
        /// Returns a room by id.
        /// </summary>
        /// <param name="id">Id of booking.</param>        
        [ProducesResponseType(typeof(Room), 200)]        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await _roomApp.GetAsync(id));
        }

        /// <summary>
        /// Returns all rooms avaibles and unavaibles.
        /// </summary>               
        [ProducesResponseType(typeof(List<Room>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _roomApp.GetAllAsync());
        }

        /// <summary>
        /// Insert a room.
        /// </summary>             
        [ProducesResponseType(typeof(int), 201)]
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Room room)
        {
            return Ok(await _roomApp.InsertAsync(room));
        }

        /// <summary>
        /// Update a room.
        /// </summary>             
        [ProducesResponseType(typeof(void), 200)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Room room)
        {
            await _roomApp.UpdateAsync(room);

            return Ok();
        }

        /// <summary>
        /// Update a room.
        /// </summary>             
        /// <param name="id">Id of room.</param>     
        [ProducesResponseType(typeof(void), 200)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _roomApp.DeleteAsync(id);

            return Ok();
        }
    }
}
