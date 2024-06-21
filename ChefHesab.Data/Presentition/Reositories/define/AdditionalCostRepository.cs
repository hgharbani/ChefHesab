using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Domain.Peresentition.IRepositories.define;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChefHesab.Domain.Peresentition.IRepositories
{
    /// <summary>
    /// سرویس مدیریت هزینه های مازاد
    /// </summary>
    public class AdditionalCostRepository : GenericRepository<AdditionalCost>, IAdditionalCostRepository
    {

        private readonly ChefHesabContext _context;
        public AdditionalCostRepository(ChefHesabContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }


        public IQueryable<AdditionalCost> GetAll()
        {
            return _context.AdditionalCosts.AsQueryable();
        }
        public IQueryable<AdditionalCost> GetAllAsNoTracking()
        {
            return _context.AdditionalCosts.AsQueryable().AsNoTracking();
        }
       
    }
}