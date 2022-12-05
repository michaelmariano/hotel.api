using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserApp _userApp;
        public UserController(IUserApp userApp)
        {
            _userApp = userApp;
        }


        [HttpPost("login")]
        public async Task<ActionResult<int>> Login([FromBody] LoginModel login)
        {
            return Ok(await _userApp.Login(login));
        }
    }
}
