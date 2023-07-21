using MealService.DTOs;
using MealService.DTOs.Wrapper;
using MealService.Services;
using Microsoft.AspNetCore.Mvc;

namespace MealService.Controllers
{
    [Route("api/cheat-meal")]
    [ApiController]
    public class CheatMealController : Controller
    {
        private readonly ICheatMealService _cheatMealService;

        public CheatMealController(ICheatMealService cheatMealService)
        {
            _cheatMealService = cheatMealService;
        }

        [Route("users/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetAll(long userId)
        {
            return Ok(new Response<IEnumerable<CheatMealDTO>>(await _cheatMealService.GetAllCheatMeals(userId)));
        }

        [Route("users/{userId}/search")]
        [HttpGet]
        public async Task<IActionResult> SearchCheatMeals(long userId, string type, int page, int size)
        {
            return Ok(new Response<IEnumerable<CheatMealDTO>>(await _cheatMealService.SearchCheatMeals(userId, type, page, size)));
        }

        [Route("users/{userId}/report")]
        [HttpGet]
        public async Task<IActionResult> SearchForReport(long userId, string fromDate, string toDate)
        {
            return Ok(new Response<IEnumerable<CheatMealDTO>>(await _cheatMealService.SearchForReport(userId, fromDate, toDate)));
        }

        [Route("users/{userId}/cheat-meals/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetCheatMealById(long userId, int id)
        {
            return Ok(new Response<CheatMealDTO>(await _cheatMealService.GetCheatMealById(userId, id)));
        }

        [Route("users/{userId}/cheat-meals/{id}")]
        [HttpDelete]
        public void RemoveCheatMeal(long userId, long id)
        {
            _cheatMealService.RemoveCheatMeal(userId, id);
        }

        [Route("users/{userId}")]
        [HttpPost]
        public async Task<IActionResult> LogCheatMeal(CheatMealDTO cheatMealDTO)
        {
            return Ok(new Response<CheatMealDTO>(await _cheatMealService.LogCheatMeal(cheatMealDTO)));
        }
    }
}
