using MealService.DTOs;
using MealService.Models;

namespace MealService.Services
{
    public interface ICheatMealReasonService
    {
        Task<IEnumerable<CheatMealReasonDTO>> GetAllCheatMealReasons();

        Task<CheatMealReasonDTO> GetCheatMealReasonById(long id);

        Task<CheatMealReason> ValidateCheatMealReason(long id);

        CheatMealReasonDTO EntityToDTO(CheatMealReason cheatMealReason);
    }
}
