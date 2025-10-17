using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.ODSViews;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.ODSViews.ViewMisJobPosition;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkFlow.BaseFile;
using Ksc.HR.DTO.ODSViews.ViewMisJobCategory;
using Ksc.HR.DTO.Personal.Employee;

namespace Ksc.HR.Appication.Services.ODSViews
{
    public class ViewMisJobPositionService : IViewMisJobPositionService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        public ViewMisJobPositionService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }
        public async Task<List<AutomCompleteModel>> GetViewMisJobPositionForAutoCompleteWF(AutoCompleteInputModel inputModel)
        {
            string text = inputModel.Text;
            //var query = (await _kscHrUnitOfWork.ViewMisUserDefinitionRepository.GetAllAsync()).Where(x => !string.IsNullOrEmpty(x.WinUser));
            var query = _kscHrUnitOfWork.ViewMisJobPositionRepository.GetAllQueryable().Where(x => x.JobPositionEndDate == 0);
            List<AutomCompleteModel> model = new List<AutomCompleteModel>();
            if (!string.IsNullOrEmpty(text))

            {
                query = query.Where(x => x.JobPositionCode.Contains(text) ||
                (x.JobTitle != null && x.JobTitle.Contains(text))).Take(20);
            }
            else
            {
                query = query.Take(20);
            }
            model = query.Select(x => new AutomCompleteModel() { text = x.JobPositionCode + "(" + x.JobTitle + ")", value = x.JobPositionCode }).ToList();
            return model;
        }

        public async Task<FilterResult<AutomCompleteModel>> GetViewMisJobPositionForConditionAutoComplete(AutoCompleteInputModel inputModel)
        {
            string text = inputModel.Text;
            var query =  _kscHrUnitOfWork.ViewMisJobPositionRepository.WhereQueryable(x => x.JobPositionEndDate == 0).AsNoTracking();
            List<AutomCompleteModel> model = new List<AutomCompleteModel>();
            var result = _FilterHandler.GetFilterResult<ViewMisJobPosition>(query, inputModel, "JobPositionCode");
            return new FilterResult<AutomCompleteModel>()
            {
                Data = result.Data.Select(x => new AutomCompleteModel() { text = x.JobPositionCode + "(" + x.JobTitle + ")", value = x.JobPositionCode }).ToList(),
                Total = result.Total

            };
        }

        public List<SearchViewMisJobStausCodeModel> GetAllViewMisJobStatusByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<ViewMisJobPosition>(_kscHrUnitOfWork.ViewMisJobPositionRepository.GetAllQueryable().AsQueryable(), Filter, nameof(ViewMisJobPosition.JobStatusCode));
            return _mapper.Map<List<SearchViewMisJobStausCodeModel>>(result.Data.DistinctBy(i => i.JobStatusCode).ToList());
        }

        public async Task<AutomCompleteModel> GetByJobPositionCode(string jobPositionCode)
        {
           var result=new AutomCompleteModel(); 
            var query = _kscHrUnitOfWork.ViewMisJobPositionRepository.FirstOrDefault(x => x.JobPositionCode == jobPositionCode);
            if (query == null)
            {
                return result;
            }
            else
            {
                result.text = query.JobPositionCode+"-"+query.JobTitle;
                result.value = query.JobPositionCode;
                return result;
            }
        }


      
    }
}
