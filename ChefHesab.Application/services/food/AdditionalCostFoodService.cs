using ChefHesab.Application.Interface;
using ChefHesab.Application.Interface.food;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Domain.Peresentition.IRepositories.food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories
{
    /// <summary>
    /// هزینه های جانبی
    /// </summary>
    public class AdditionalCostFoodService : IAdditionalCostFoodService
    {
        public IChefHesabUnitOfWork _unitOfWork;

        public AdditionalCostFoodService(IChefHesabUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       

    }
}
