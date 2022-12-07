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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await _roomApp.GetAsync(id));
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _roomApp.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Room room)
        {
            return Ok(await _roomApp.InsertAsync(room));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Room room)
        {
            await _roomApp.UpdateAsync(room);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _roomApp.DeleteAsync(id);

            return Ok();
        }
    }
}
