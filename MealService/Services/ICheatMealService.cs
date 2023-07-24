using MealService.DTOs;

namespace MealService.Services
{
    public interface ICheatMealService
    {
        Task<CheatMealDTO> LogCheatMeal(long userId, CheatMealDTO cheatMealDTO);

        Task<IEnumerable<CheatMealDTO>> SearchCheatMeals(long userId, string mealType, int page, int size);

        Task<IEnumerable<CheatMealDTO>> GetAllCheatMeals(long userId);

        Task<CheatMealDTO> GetCheatMealById(long userId, long id);

        void RemoveCheatMeal(long userId, long id);

        Task<IEnumerable<CheatMealDTO>> SearchForReport(long userId, string fromDate, string toDate);
    }
}
