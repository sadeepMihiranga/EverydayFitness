using MealService.DTOs;

namespace MealService.Services
{
    public interface ICheatMealTypeService
    {
        Task<IEnumerable<CheatMealTypeDTO>> GetAllCheatMealTypes();

        Task<CheatMealTypeDTO> GetCheatMealTypeById(int id);
    }
}
