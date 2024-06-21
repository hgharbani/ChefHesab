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
using Microsoft.Extensions.Logging;

namespace ChefHesab.Data.Presentition.Reositories.define
{
    public class AuthenticateRepository : GenericRepository<Authenticate>, IAuthenticateRepository
    {
        private readonly ChefHesabContext _context;
        public AuthenticateRepository(ChefHesabContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

    }
}
