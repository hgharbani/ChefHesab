using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.ODSViews;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.ODSViews.ViewMisCostCenter;
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
    public class ViewMisCostCenterService : IViewMisCostCenterService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        public ViewMisCostCenterService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }
        public List<SearchViewMisCostCenterModel> GetViewMisCostCenterByKendoFilter(FilterRequest Filter)
        {
            var query = _kscHrUnitOfWork.ViewMisCostCenterRepository.GetAllQueryable().AsQueryable();
            var result = _FilterHandler.GetFilterResult<ViewMisCostCenter>(query, Filter, nameof(ViewMisCostCenter.CostCenterCode));
            var finalData = result.Data.Select(a => new SearchViewMisCostCenterModel()
            {
                CostCenterCode = a.CostCenterCode,
                CostCenterTitle = a.CostCenterCode+"-"+ a.CostCenterTitle
            }).ToList();
            return _mapper.Map<List<SearchViewMisCostCenterModel>>(finalData);
        }


    }
}
