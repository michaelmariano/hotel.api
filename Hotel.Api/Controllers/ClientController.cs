﻿using Application.Interfaces;
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

        /// <summary>
        /// Insert a client.
        /// </summary>            
        [ProducesResponseType(typeof(int), 201)]
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Client client)
        {
            return Created("/api/client/login", await _clientApp.Insert(client));
        }

        /// <summary>
        /// Returns a client if login has been succeed.
        /// </summary>           
        [ProducesResponseType(typeof(Client), 200)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            return Ok(await _clientApp.Login(login));
        }
    }
}
