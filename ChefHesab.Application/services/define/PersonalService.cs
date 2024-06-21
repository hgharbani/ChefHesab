using AutoMapper;
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
    /// <summary>
    /// سرویس مدیریت افراد
    /// </summary>
    public class PersonalService : IPersonalService
    {
        public IChefHesabUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PersonalService(IChefHesabUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<List<Personal>> GetAllProducts()
        {
            var PersonalList = _unitOfWork.PersonalRepository.GetAllAsync();
            return PersonalList;
        }
    }
}


     
