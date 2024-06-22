using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;

namespace ChefHesab.Domain.Peresentition.IRepositories.food
{
    public class FoodProviderRepository : GenericRepository<FoodProvider>, IFoodProviderRepository
    {
        public FoodProviderRepository(ChefHesabContext chefHesab) : base(chefHesab)
        {
        }


    }
}