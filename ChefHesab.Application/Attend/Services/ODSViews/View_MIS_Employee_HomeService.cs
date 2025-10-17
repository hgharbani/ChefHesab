using AutoMapper;
using Ksc.Hr.Application.Interfaces;
using Ksc.Hr.Domain.Repositories;
using Ksc.Hr.DTO.View_MIS_Employee_Home;
using Ksc.HR.Domain.Repositories;
using KSC.Common.Filters.Contracts;
using System.Linq;

namespace Ksc.Hr.Application.Services
{


    public class View_MIS_Employee_HomeService : IView_MIS_Employee_HomeService
    {
        private readonly IMapper _mapper;
        private readonly IFilterHandler _filterHandler;
        private readonly IKscHrUnitOfWork _repository;
        public View_MIS_Employee_HomeService(IMapper mapper, IFilterHandler filterHandler, IKscHrUnitOfWork repository)
        {
            _filterHandler = filterHandler;
            _repository = repository;
            _mapper = mapper;

        }

        public View_MIS_Employee_HomeDto GetView_MIS_Employee_Home(string employeenumber)
        {
            
            var findemployeehome = _repository.View_MIS_Employee_HomeRepository.Where(a => a.EmployeeNumber == employeenumber)
                .Select(a => _mapper.Map<View_MIS_Employee_HomeDto>(a)).FirstOrDefault();
            return findemployeehome;
        }

    }
}



