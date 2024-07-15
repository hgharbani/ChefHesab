using ChefHesab.Dto.food.AdditionalCostFood;
using ChefHesab.Share.model;

namespace ChefHesab.Application.Interface.food
{
    public interface IAdditionalCostFoodService
    {
        Task<ChefResult> Create(CreateAdditionalCostFoodVM model);
        Task<ChefResult> Delete(SearchAdditionalCostfoodVM model);
        dynamic GetAdditionalCostFood(SearchAdditionalCostfoodVM request);
    }
}