using ChefHesab.Data;
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
        public AdditionalCostRepository(ChefHesabContext chefHesab):base(chefHesab)
        {
                
        }


        public IQueryable<AdditionalCost> GetAll()
        {
            return _dbContext.Set<AdditionalCost>().AsQueryable();
        }
        public IQueryable<AdditionalCost> GetAllAsNoTracking()
        {
            return _dbContext.Set<AdditionalCost>().AsQueryable().AsNoTracking();
        }
       
    }
}