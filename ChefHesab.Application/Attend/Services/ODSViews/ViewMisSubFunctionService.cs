using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.ODSViews;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.ODSViews.ViewMisSubFunction;
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
    public class ViewMisSubFunctionService : IViewMisSubFunctionService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        public ViewMisSubFunctionService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }
        public List<SearchViewMisSubFunctionModel> GetCurrentViewMisSubFunctionByKendoFilter(FilterRequest Filter)
        {
            var query = _kscHrUnitOfWork.ViewMisSubFunctionRepository.GetAllQueryable().
                Where(x => x.IsActiveCostCenter == 1 && x.EndDateSection == 0 && x.LengthSectionCode == 4)
                .AsQueryable();
            var result = _FilterHandler.GetFilterResult<ViewMisSubFunction>(query, Filter, nameof(ViewMisSubFunction.CostCenterCode));
            var finalData = result.Data.Select(a => new SearchViewMisSubFunctionModel()
            {
                SectionCode = a.SectionCode,
                Section = a.Section
            }).ToList();
            return _mapper.Map<List<SearchViewMisSubFunctionModel>>(finalData);
        }


    }
}
