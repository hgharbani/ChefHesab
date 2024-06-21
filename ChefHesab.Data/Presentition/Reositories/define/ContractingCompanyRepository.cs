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



    public class ContractingCompanyRepository :GenericRepository<ContractingCompany>, IContractingCompanyRepository
    {

        private readonly ChefHesabContext _context;
        public ContractingCompanyRepository(ChefHesabContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public IQueryable<ContractingCompany> GetAllAsNoTracking()
        {
            return _context.ContractingCompanies.AsNoTracking();
        }

    }
}