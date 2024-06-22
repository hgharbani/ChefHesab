using ChefHesab.Data;
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

        public ContractingCompanyRepository(ChefHesabContext chefHesab) : base(chefHesab)
        {
            
        }
        public IQueryable<ContractingCompany> GetAllAsNoTracking()
        {
            return _dbContext.Set<ContractingCompany>().AsNoTracking();
        }

    }
}