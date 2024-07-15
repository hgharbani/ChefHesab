using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using Microsoft.EntityFrameworkCore;

namespace ChefHesab.Domain.Peresentition.IRepositories.food
{
    public class FoodProviderRepository : GenericRepository<FoodProvider>, IFoodProviderRepository
    {
        public FoodProviderRepository(ChefHesabContext chefHesab) : base(chefHesab)
        {
        }

        public IQueryable<FoodProvider> GetAll()
        {
            return _dbContext.Set<FoodProvider>().AsQueryable();
        }
        public IQueryable<FoodProvider> GetAllAsNoTracking()
        {
            return _dbContext.Set<FoodProvider>().AsQueryable().AsNoTracking();
        }

    }
}