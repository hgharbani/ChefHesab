using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using Microsoft.EntityFrameworkCore;

namespace ChefHesab.Domain.Peresentition.IRepositories.define
{
    public class FoodStuffRepository : GenericRepository<FoodStuff>, IFoodStuffRepository
    {
        public FoodStuffRepository(ChefHesabContext chefHesab) : base(chefHesab)
        {
            
        }
        public IQueryable<FoodStuff> GetAllQuery()
        {
            return SelectQuery().Include(a => a.FoodCategory).Include(a => a.StuffPrices).AsQueryable();
        }



    }
}