using ChefHesab.Data.Presentition.Context;
using ChefHesab.Domain;
using ChefHesab.Domain.Peresentition.IRepositories;
using ChefHesab.Data.Presentition.Reositories.generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ChefHesab.Data.Presentition.Reositories.define
{
    public class PersonalRepository : GenericRepository<Personal>,IPersonalRepository
    {
        public PersonalRepository(ChefHesabContext chefHesab) : base(chefHesab)
        {
            
        }
        public async Task<List<Personal>> GetAllAsync()
        {
            return await _dbContext.Set<Personal>().ToListAsync();
        }
    }
}
