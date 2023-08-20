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

namespace ChefHesab.Data.Presentition.Reositories.define
{
    public class PersonalRepository : GenericRepository<Personal>,IPersonalRepository
    {
        private readonly ChefHesabContext _context;
        public PersonalRepository(ChefHesabContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }


        public async Task<List<Personal>> GetAllAsync()
        {
            return await _context.Personals.ToListAsync();
        }
    }
}
