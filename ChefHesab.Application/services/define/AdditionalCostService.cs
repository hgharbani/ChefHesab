using ChefHesab.Application.Interface;
using ChefHesab.Application.Interface.define;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Domain;
using ChefHesab.Domain.Peresentition.IRepositories;
using ChefHesab.Domain.Peresentition.IRepositories.define;
using ChefHesab.Domain.Peresentition.IRepositories.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Application.services.define
{
    public class AdditionalCostService : IAdditionalCostService
    {
        public IChefHesabUnitOfWork _unitOfWork;

        public AdditionalCostService(IChefHesabUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public List<AdditionalCost> GetAdditionalCost()
        {
            return _unitOfWork.AdditionalCostRepository.SelectAll().ToList();
        }



    }
}
