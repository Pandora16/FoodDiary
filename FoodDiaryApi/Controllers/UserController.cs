using Core.Interfaces.Services; 
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Business;
using System.Collections.Generic;

// Контроллер обрабатывает HTTP-запросы
// HTTP-запросы — это сообщения, отправляемые клиентом (например, веб-браузером или приложением) на сервер,
// который обрабатывает эти запросы и возвращает соответствующие ответы

namespace FoodDiaryApi.Controllers
{

    [ApiController]
    // Задает маршрут к контроллеру
    [Route("api/[controller]")] 
    // Этот класс является контроллером, который обрабатывает HTTP-запросы, связанные с объектами пользователей (User)
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService; 

        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        // POST: Отправляет данные на сервер для создания нового ресурса (например, добавить нового пользователя )
        [HttpPost]
        // [FromBody] - Используется для передачи: Сложных объектов (например, User, Food)
        public async Task<IActionResult> AddUser([FromBody] User user) 
        {
            if (user == null)
            {
                // метод BadRequest используется, если клиент отправил некорректные данные.
                // В скобках можно передать Строку с описанием проблемы
                return BadRequest("Invalid user data.");
            }

            await _userService.AddUserAsync(user);
            // CreatedAtAction: Возвращает HTTP-статус 201 Created (успешное создание ресурса)
            return CreatedAtAction(nameof(LoadUsers), new { id = user.Id }, user);
        }

        [HttpGet] // GET: Запрашивает данные с сервера о пользователях
        public async Task<IActionResult> LoadUsers()
        {
            var users = await _userService.LoadUsersAsync();
            // Ok() возвращает стандартный HTTP-ответ с кодом 200 OK. Используется для успешного завершения запроса
            // "200 OK" является статусным кодом, который указывает на успешное выполнение запроса
            return Ok(users); // users передаёт данные, которые будут включены в тело ответа - cписок пользователей
        }
    }
}