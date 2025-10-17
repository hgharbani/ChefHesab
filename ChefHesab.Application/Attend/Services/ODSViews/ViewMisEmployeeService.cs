using AutoMapper;
using Ksc.HR.Appication.Interfaces.ODSViews;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.ODSViews.ViewMisEmployee;
using KSC.Common.Filters.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.ODSViews
{
    public class ViewMisEmployeeService : IViewMisEmployeeService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public ViewMisEmployeeService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler filterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = filterHandler;
        }

        public List<SearchViewMisEmployee> GetViewMisEmployeeByKendoFilter(SearchViewMisEmployee filter)
        {
            var queryMisEmployees = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetMisEmployeesByJobPositionCode(filter.JobPositionCode);
            var result = _FilterHandler.GetFilterResult<ViewMisEmployee>(queryMisEmployees, filter, nameof(ViewMisEmployee.EmployeeNumber));
            var finalData = result.Data.Select(x => new SearchViewMisEmployee
            {
                EmployeeNumber = x.EmployeeNumber,
                FirstName = x.FirstName.Trim(),
                LastName = x.LastName.Trim(),
                JobCategoryCode = x.JobCategoryCode,
                JobCategoryTitle = x.JobCategoryTitle,
                JobPositionCode = x.JobPositionCode,
                JobPositionTitle = x.JobPositionTitle,
                JobStatusCode = x.JobStatusCode,
                PaymentStatusCode = x.PaymentStatusCode,
                MisUser = x.MisUser.Trim(),
                WinUser = x.WinUser.Trim()
            });
            var data = _mapper.Map<List<SearchViewMisEmployee>>(finalData);
            return data;
        }
        public async Task<ViewMisEmployeeModel> GetViewEmployeeByEmployeeNumber(string employeeNumber)
        {
            var model = await _kscHrUnitOfWork.ViewMisEmployeeRepository.GetMisEmployeesByWinUser(employeeNumber);
            if (model == null)
                return new ViewMisEmployeeModel() { CategoryId=0,JobPositionCode="00"};
            return _mapper.Map<ViewMisEmployeeModel>(model);
        }

        //public async Task<ViewMisEmployeeModel> GetViewEmployeeByEmployeeNumber_Ver1(string employeeNumber)
        //{
        //    var model = await _kscHrUnitOfWork.View_EmployeeRepository.GetEmployeesByWinUser(employeeNumber);
        //    if (model == null)
        //        return new ViewMisEmployeeModel() { CategoryId = 0, JobPositionCode = "00" };
        //    return new ViewMisEmployeeModel { 
        //        CategoryId = model.CategoryId , 
        //        CategoryTitle = model.CategoryTitle , 
        //        EmployeeNumber = model.EmployeeNumber , 
        //        JobPositionCode = model.MisJobPositionCode , 
        //        JobPositionTitle = model.JobPositionTitle };
        //}


        public async Task<ViewEmployeeInfoModel> GetEmployeeInfoByUserWin(string userWin)
        {
            var model = await _kscHrUnitOfWork.ViewMisEmployeeRepository.GetMisEmployeesByWinUser(userWin);
            if (model == null)
                return new ViewEmployeeInfoModel();
            return _mapper.Map<ViewEmployeeInfoModel>(model);
        }

      
        public List<string> GetListUserNameByJob(string filter)
        {
          
            var find = _kscHrUnitOfWork.ViewMisEmployeeRepository
                .Where(a => a.JobPositionCode == filter).Select(a=>a.WinUser).ToList();
            
            return find;
        }
        public List<string> GetListUserNameByCategory(int categoryId)
        {
          
            var find = _kscHrUnitOfWork.ViewMisEmployeeRepository
                .Where(a =>a.CategoryId == categoryId).Select(a => a.WinUser).ToList();

            return find;
        }

        public async Task<List<ViewMisEmployeeForFinancialModel>> GetEmployeeForFinancialByEmployeeNumbers(SearchEmployeeForFinancial filter)
        {
            var model = _kscHrUnitOfWork.ViewMisEmployeeRepository.GetEmployeeByPersonalNumbers(filter.EmployeeNumber);
            if (model == null)
                return new List<ViewMisEmployeeForFinancialModel>();
            return _mapper.Map<List<ViewMisEmployeeForFinancialModel>>(model);
        }
    }
}
