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
        public async Task<Room?> Get([FromRoute] int id)
        {
            return await _roomApp.GetAsync(id);
        }

        [HttpGet()]
        public async Task<List<Room>> GetAll()
        {
            return await _roomApp.GetAllAsync();
        }

        [HttpPost]
        public async Task<int> Insert([FromBody] Room room)
        {
            return await _roomApp.InsertAsync(room);
        }

        [HttpPut]
        public async Task Update([FromBody] Room room)
        {
            await _roomApp.UpdateAsync(room);
        }

        [HttpDelete("{id}")]
        public async Task Delete([FromRoute] int id)
        {
            await _roomApp.DeleteAsync(id);
        }
    }
}
