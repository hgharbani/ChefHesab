using ChefHesab.Application.Interface;
using ChefHesab.Application.Interface.food;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories.food
{
    /// <summary>
    /// سرویس مدیریت مواد لازم هر پرس غذا
    /// </summary>
    public class IngredinsFoodService : IIngredinsFoodService
    {
        public IChefHesabUnitOfWork _unitOfWork;

        public IngredinsFoodService(IChefHesabUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void get(int id)
        {

        }

    }
}
