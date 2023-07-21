using MealService.DTOs;
using MealService.Models;

namespace MealService.Services
{
    public interface ICheatMealTypeService
    {
        Task<IEnumerable<CheatMealTypeDTO>> GetAllCheatMealTypes();

        Task<CheatMealTypeDTO> GetCheatMealTypeById(long id);

        Task<CheatMealType> ValidateCheatMealType(long id);

        CheatMealTypeDTO EntityToDTO(CheatMealType cheatMealType);
    }
}
