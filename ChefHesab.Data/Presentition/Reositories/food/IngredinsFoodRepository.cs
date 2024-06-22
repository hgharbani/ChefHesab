using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;

namespace ChefHesab.Domain.Peresentition.IRepositories.food
{
    public class IngredinsFoodRepository : GenericRepository<IngredinsFood>, IIngredinsFoodRepository
    {
        public IngredinsFoodRepository(ChefHesabContext chefHesab) : base(chefHesab)
        {
        }

    }
}