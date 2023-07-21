using MealService.DTOs;
using MealService.DTOs.Wrapper;
using MealService.Services;
using Microsoft.AspNetCore.Mvc;

namespace MealService.Controllers
{
    [Route("api/cheat-meal-reason")]
    [ApiController]
    public class CheatMealReasonController : Controller
    {
        private readonly ICheatMealReasonService _cheatMealReasonService;

        public CheatMealReasonController(ICheatMealReasonService cheatMealReasonService)
        {
            _cheatMealReasonService = cheatMealReasonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new Response<IEnumerable<CheatMealReasonDTO>>(await _cheatMealReasonService.GetAllCheatMealReasons()));
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GeById(long id)
        {
            return Ok(new Response<CheatMealReasonDTO>(await _cheatMealReasonService.GetCheatMealReasonById(id)));
        }
    }
}
