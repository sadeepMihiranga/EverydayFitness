using MealService.DTOs;
using MealService.DTOs.Wrapper;
using MealService.Services;
using Microsoft.AspNetCore.Mvc;

namespace MealService.Controllers
{
    [Route("api/cheat-meal-type")]
    [ApiController]
    public class CheatMealTypeController : Controller
    {
        private readonly ICheatMealTypeService _cheatMealTypeService;

        public CheatMealTypeController(ICheatMealTypeService cheatMealTypeService)
        {
            _cheatMealTypeService = cheatMealTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new Response<IEnumerable<CheatMealTypeDTO>>(await _cheatMealTypeService.GetAllCheatMealTypes()));
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GeById(int id)
        {
            return Ok(new Response<CheatMealTypeDTO>(await _cheatMealTypeService.GetCheatMealTypeById(id)));
        }
    }
}
