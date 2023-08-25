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
    public class FoodStuffRepository : GenericRepository<FoodStuff>, IFoodStuffRepository
    {
        private readonly ChefHesabContext _context;
        public FoodStuffRepository(ChefHesabContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

       public IQueryable<FoodStuff> GetAllQuery()
        {
            return _context.FoodStuffs.Include(a => a.FoodCategory).AsQueryable();
        }



    }
}