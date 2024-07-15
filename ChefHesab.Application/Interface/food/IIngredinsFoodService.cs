using ChefHesab.Dto.food.IngredinsFood;
using ChefHesab.Share.model;

namespace ChefHesab.Application.Interface.food
{
    public interface IIngredinsFoodService
    {
        Task<ChefResult> Create(CreateIngredinsFoodVM model);
        Task<ChefResult> Delete(SearchIngredinsFoodVm model);
        dynamic GetIngredinsFoods(SearchIngredinsFoodVm request);
    }
}