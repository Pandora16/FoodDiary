using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Business;

namespace FoodDiaryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;
        private readonly ICalculateStatisticsService _calculateStatisticsService;
        private readonly IUserService _userService;

        public FoodController(IFoodService foodService, IUserService userService, ICalculateStatisticsService calculateStatisticsService) 
        {
            _foodService = foodService;
            _userService = userService;
            _calculateStatisticsService = calculateStatisticsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFood([FromBody] Food food)
        {
            if (food == null)
            {
                return BadRequest("Invalid food data.");
            }

            await _foodService.AddFoodAsync(food);
            return CreatedAtAction(nameof(GetAllFoods), new { id = food.Id }, food);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFoods()
        {
            var foods = await _foodService.GetAllFoodsAsync();
            return Ok(foods);
        }

        [HttpPost("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] string userName, [FromQuery] string choice)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(choice))
            {
                return BadRequest("Invalid user name or choice.");
            }

            var users = await _userService.LoadUsersAsync();
            var user = users.FirstOrDefault(u => u.Name == userName);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _calculateStatisticsService.CalculateStatistic(user, choice);
            return Ok(result);
        }
    }
}