using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Domain.Peresentition.IRepositories.food;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories
{
    public class AdditionalCostFoodRepository : GenericRepository<AdditionalCostFood>, IAdditionalCostFoodRepository
    {


        private readonly ChefHesabContext _context;
        public AdditionalCostFoodRepository(ChefHesabContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }



    }
}