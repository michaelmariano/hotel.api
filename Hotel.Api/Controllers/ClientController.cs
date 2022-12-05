using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IUserApp _userApp;
        public ClientController(IUserApp userApp)
        {
            _userApp = userApp;
        }

        //[HttpGet("get/{id}")]
        //public async Task<User> GetByIdAsync(int id)
        //{
        //    return await _userApp.GetByIdAsync(id);
        //}
    }
}
