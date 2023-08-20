using ChefHesab.Application.Interface.define;
using ChefHesab.Domain;
using ChefHesab.Domain.Peresentition.IRepositories;
using Dalir.common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Application.services.define
{
    public class PersonalService : IPersonalService
    {
        public IChefHesabUnitOfWork _unitOfWork;

        public PersonalService(IChefHesabUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<List<Personal>> GetAllProducts()
        {
            var PersonalList = await _unitOfWork.PersonalRepository.GetAllAsync();
            return PersonalList;
        }



    }
}
