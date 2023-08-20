using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories.food
{
    public class IngredinsFoodRepository : GenericRepository<IngredinsFood>, IIngredinsFoodRepository
    {
        private readonly ChefHesabContext _context;
        public IngredinsFoodRepository(ChefHesabContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }


      
    }
}