using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories.define
{
    public class FoodCategoryRepository : GenericRepository<FoodCategory>, IFoodCategoryRepository
    {

        private readonly ChefHesabContext _context;
        public FoodCategoryRepository(ChefHesabContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }


        public IQueryable<FoodCategory> GetAllQueryble()
        {
            return GetAllQueryable();
        }
      
    }
}