using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Business;
using static System.Net.WebRequestMethods;

// Контроллер обрабатывает HTTP-запросы
// HTTP-запросы — это сообщения, отправляемые клиентом (например, веб-браузером или приложением) на сервер,
// который обрабатывает эти запросы и возвращает соответствующие ответы

namespace FoodDiaryApi.Controllers 
                                   
{
    
    [ApiController]
    // Задает маршрут к контроллеру
    [Route("api/[controller]")]
    // Этот класс является контроллером, который обрабатывает HTTP-запросы, связанные с объектами еды (Food)
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

        // POST: Отправляет данные на сервер для создания нового ресурса (например, добавить новую еду )
        [HttpPost]
        // [FromBody] - Используется для передачи: Сложных объектов (например, User, Food)
        public async Task<IActionResult> AddFood([FromBody] Food food) 
        {
            if (food == null)
            {
                // метод BadRequest используется, если клиент отправил некорректные данные.
                // В скобках можно передать Строку с описанием проблемы
                return BadRequest("Invalid food data.");
            }

            await _foodService.AddFoodAsync(food);
            // CreatedAtAction: Возвращает HTTP-статус 201 Created (успешное создание ресурса)
            return CreatedAtAction(nameof(GetAllFoods), new { id = food.Id }, food);
        }

        [HttpGet] // GET: Запрашивает данные с сервера о еде
        public async Task<IActionResult> GetAllFoods()
        {
            var foods = await _foodService.GetAllFoodsAsync();
            // Ok() возвращает стандартный HTTP - ответ с кодом 200 OK.Используется для успешного завершения запроса
            // "200 OK" является статусным кодом, который указывает на успешное выполнение запроса
            return Ok(foods); // result передаёт данные, которые будут включены в тело ответа - еду
        }

        //
        [HttpGet("statistics")] 
        // [FromQuery] - Используется для передачи: Простых данных (например, строк, чисел). В статистике как раз такое.
        public async Task<IActionResult> GetStatistics([FromQuery] string userName, [FromQuery] string choice)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(choice))
            {
                // метод BadRequest используется, если клиент отправил некорректные данные.
                // В скобках можно передать Строку с описанием проблемы
                return BadRequest("Invalid user name or choice."); 
            }

            var users = await _userService.LoadUsersAsync();
            var user = users.FirstOrDefault(u => u.Name == userName);
            if (user == null)
            {
                // метод NotFound() возвращается, если запрашиваемый ресурс не найден. 
                // В скобках можно указать Строку с указанием, что именно не найдено
                return NotFound("User not found.");
            }
            var result = await _calculateStatisticsService.CalculateStatistic(user, choice);
            return Ok(result); // result передаёт данные, которые будут включены в тело ответа - статистика пользователя
        }
    }
}