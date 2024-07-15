using ChefHesab.Dto.food.FoodProvider;
using ChefHesab.Share.model;

namespace ChefHesab.Application.Interface.food
{
    public interface IFoodProviderService
    {
        Task<ChefResult> Add(CreateFoodProviderVM model);
        Task<ChefResult> Edit(CreateFoodProviderVM model);
        dynamic GetFoodProviders(SearchfoodProvider request);
        Task<CreateFoodProviderVM> GetForEdit(Guid id);
        Task<FoodProviderVM> GetHeaderAnalyzeFood(Guid id);
    }
}