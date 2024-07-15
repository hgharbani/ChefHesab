using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Dto.food.StuffPrice;
using ChefHesab.Share.model;

namespace ChefHesab.Application.Interface.food
{
    public interface IStuffPriceService
    {
        Task<ChefResult> AddOrUpdate(CreateStuffPriceVM model);
        dynamic GetIngredinsFoods(FoodStuffSearch request);
    }
}