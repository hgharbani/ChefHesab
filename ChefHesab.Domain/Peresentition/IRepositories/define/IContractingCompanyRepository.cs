using ChefHesab.Domain.Peresentition.IRepositories.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories.define
{
    public interface IContractingCompanyRepository  : IGenericRepository<ContractingCompany>
    {
        void Add(ContractingCompany company);
        Task<bool> Any(Func<object, bool> value);
        IQueryable<ContractingCompany> GetAll();
        IQueryable<ContractingCompany> GetAllAsNoTracking();
    }
}
