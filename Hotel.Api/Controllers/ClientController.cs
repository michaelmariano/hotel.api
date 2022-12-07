using Application.Interfaces;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientApp _clientApp;

        public ClientController(IClientApp clientApp)
        {
            _clientApp = clientApp;
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Client client)
        {
            return Created("/api/client/login", await _clientApp.Insert(client));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            return Ok(await _clientApp.Login(login));
        }
    }
}
