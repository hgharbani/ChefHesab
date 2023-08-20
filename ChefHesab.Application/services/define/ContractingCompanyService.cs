using ChefHesab.Application.Interface;
using ChefHesab.Application.Interface.define;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories.define
{
    public class ContractingCompanyService : IContractingCompanyService
    {
        public IChefHesabUnitOfWork _unitOfWork;

        public ContractingCompanyService(IChefHesabUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void get(int id)
        {

        }

    }
}
