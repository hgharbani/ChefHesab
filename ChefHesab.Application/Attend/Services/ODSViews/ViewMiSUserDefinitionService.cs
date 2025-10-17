using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.ODSViews;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.ODSViews;
using Ksc.HR.DTO.ODSViews.ViewMisUserDefinition;
using Ksc.HR.DTO.WorkFlow.BaseFile;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.ODSViews
{
    public class ViewMiSUserDefinitionService : IViewMiSUserDefinitionService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        private readonly IViewMisUserDefinitionRepository _viewMiSUserDefinitionRepository;
        public ViewMiSUserDefinitionService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler, IViewMisUserDefinitionRepository viewMiSUserDefinitionRepository)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
            _viewMiSUserDefinitionRepository = viewMiSUserDefinitionRepository;
        }
        public FilterResult<SearchViewMisUserDefinitionModel> GetViewMiSUserDefinitionByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<ViewMisUserDefinition>(_kscHrUnitOfWork.ViewMisUserDefinitionRepository.GetAllQueryable().AsQueryable(), Filter, nameof(ViewMisUserDefinition.WinUser));
            return new FilterResult<SearchViewMisUserDefinitionModel>()
            {
                Data = _mapper.Map<List<SearchViewMisUserDefinitionModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }

        public async Task<List<AutomCompleteModel>> GetMiSUserDefinitionForAutoCompleteWF(AutoCompleteInputModel inputModel)
        {
            string text = inputModel.Text;
            //var query = (await _kscHrUnitOfWork.ViewMisUserDefinitionRepository.GetAllAsync()).Where(x => !string.IsNullOrEmpty(x.WinUser));
            var query = _viewMiSUserDefinitionRepository.WhereQueryable(x => !string.IsNullOrEmpty(x.WinUser));
            List<AutomCompleteModel> model = new List<AutomCompleteModel>();
            if (!string.IsNullOrEmpty(text))

            {
                query = query.Where(x => (x.PersonalNumber != null && x.PersonalNumber.Value.ToString().Contains(text)) ||
                (x.FirstName!=null && x.FirstName.Contains(text)) || 
               (x.LastName!=null && x.LastName.Contains(text) )||
                x.WinUser.ToLower().Contains(text.ToLower())).Take(20);
            }
            else
            {
                query = query.Take(20);
            }
            model = query.Select(x => new AutomCompleteModel() { text = x.WinUser + "(" + x.FirstName + " " + x.LastName + ")", value = x.WinUser.Trim() }).ToList();
            return model;
        }
    }
}
