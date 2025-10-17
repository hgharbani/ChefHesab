using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.ODSViews;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.ODSViews.ViewMisEmploymentType;
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
    public class ViewMisEmploymentTypeService : IViewMisEmploymentTypeService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        public ViewMisEmploymentTypeService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }
        public List<SearchViewMisEmploymentTypeModel> GetViewMisEmploymentTypeByKendoFilter(FilterRequest Filter)
        {
            var query = _kscHrUnitOfWork.ViewMisEmploymentTypeRepository.GetAllQueryable().AsQueryable();
            var result = _FilterHandler.GetFilterResult<ViewMisEmploymentType>(query, Filter, nameof(ViewMisEmploymentType.EmploymentTypeCode));
            var finalData = result.Data.Select(a => new SearchViewMisEmploymentTypeModel()
            {
                EmploymentTypeCode = a.EmploymentTypeCode,
                EmploymentTypeTitle = a.EmploymentTypeTitle
            }).ToList();
            return _mapper.Map<List<SearchViewMisEmploymentTypeModel>>(finalData);
        }


    }
}
