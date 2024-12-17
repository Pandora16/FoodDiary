using Core.Interfaces.Services; // Обязательно добавьте этот using
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Business;
using System.Collections.Generic;

namespace FoodDiaryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService; 

        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(LoadUsers), new { id = user.Id }, user);
        }

        [HttpGet]
        public async Task<IActionResult> LoadUsers()
        {
            var users = await _userService.LoadUsersAsync();
            return Ok(users);
        }
    }
}