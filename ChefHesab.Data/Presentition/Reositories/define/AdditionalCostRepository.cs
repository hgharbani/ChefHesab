using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Domain.Peresentition.IRepositories.define;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories
{
    public class AdditionalCostRepository : GenericRepository<AdditionalCost>, IAdditionalCostRepository
    {
        private readonly ChefHesabContext _context;
        public AdditionalCostRepository(ChefHesabContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }


       
    }
}