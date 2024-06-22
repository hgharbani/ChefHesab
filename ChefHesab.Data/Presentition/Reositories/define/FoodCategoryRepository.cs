using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;

namespace ChefHesab.Domain.Peresentition.IRepositories.define
{
    public class FoodCategoryRepository : GenericRepository<FoodCategory>, IFoodCategoryRepository
    {
        public FoodCategoryRepository(ChefHesabContext chefHesab) : base(chefHesab)
        {

        }
        public IQueryable<FoodCategory> GetAllQueryble()
        {
            return _dbContext.Set<FoodCategory>().AsQueryable();
        }

    }
}