using Microsoft.AspNetCore.Mvc;
using WorkoutService.DTOs;
using WorkoutService.DTOs.Wrapper;
using WorkoutService.Services;

namespace WorkoutService.Controllers
{
    [Route("api/workout-type")]
    [ApiController]
    public class WorkoutTypeController : Controller
    {
        private readonly IWorkoutTypeService _workoutTypeService;

        public WorkoutTypeController(IWorkoutTypeService workoutTypeService)
        {
            _workoutTypeService = workoutTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new Response<IEnumerable<WorkoutTypeDTO>>(await _workoutTypeService.GetAllWorkoutTypes()));
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GeById(int id)
        {
            return Ok(new Response<WorkoutTypeDTO>(await _workoutTypeService.GetWorkoutTypeById(id)));
        }
    }
}
