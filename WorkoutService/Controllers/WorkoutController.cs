using Microsoft.AspNetCore.Mvc;
using WorkoutService.DTOs;
using WorkoutService.DTOs.Wrapper;
using WorkoutService.Services;

namespace WorkoutService.Controllers
{
    [Route("api/workout")]
    [ApiController]
    public class WorkoutController : Controller
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [Route("users/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetAll(long userId)
        {
            return Ok(new Response<IEnumerable<WorkoutDTO>>(await _workoutService.GetAllWorkouts(userId)));
        }

        [Route("users/{userId}/search")]
        [HttpGet]
        public async Task<IActionResult> SearchWorkouts(long userId, string? type, int page, int size)
        {
            return Ok(new Response<IEnumerable<WorkoutDTO>>(await _workoutService.SearchWorkouts(userId, type, page, size)));
        }

        [Route("users/{userId}/report")]
        [HttpGet]
        public async Task<IActionResult> SearchForReport(long userId, string fromDate, string toDate)
        {
            return Ok(new Response<IEnumerable<WorkoutDTO>>(await _workoutService.SearchForReport(userId, fromDate, toDate)));
        }

        [Route("users/{userId}/workouts/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetWorkoutById(long userId, int id)
        {
            return Ok(new Response<WorkoutDTO>(await _workoutService.GetWorkoutById(userId, id)));
        }

        [Route("users/{userId}/workouts/{id}")]
        [HttpDelete]
        public void RemoveWorkout(long userId, long id)
        {
            _workoutService.RemoveWorkout(userId, id);
        }

        [Route("users/{userId}")]
        [HttpPost]
        public async Task<IActionResult> LogWorkout(long userId, WorkoutDTO workoutDTO)
        {
            return Ok(new Response<WorkoutDTO>(await _workoutService.LogWorkout(userId, workoutDTO)));
        }

        [Route("users/{userId}/workouts/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateWorkout(long userId, long id, WorkoutDTO workoutDTO)
        {
            return Ok(new Response<WorkoutDTO>(await _workoutService.UpdateWorkout(userId, id, workoutDTO)));
        }
    }
}
