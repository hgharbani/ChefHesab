using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.ODSViews;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.ODSViews.ViewMisSalaryCode;
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
    public class ViewMisSalaryCodeService : IViewMisSalaryCodeService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        public ViewMisSalaryCodeService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public List<SearchViewMisSalaryCodeModel> GetAllViewMisSalaryCodeByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<ViewMisSalaryCode>(_kscHrUnitOfWork.ViewMisSalaryCodeRepository.GetAllQueryable().AsQueryable(), Filter, nameof(ViewMisSalaryCode.SalaryAccountCode));
            return _mapper.Map<List<SearchViewMisSalaryCodeModel>>(result.Data.ToList());
        }

        public FilterResult<SearchViewMisSalaryCodeModel> GetViewMisSalaryCodeByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<ViewMisSalaryCode>(_kscHrUnitOfWork.ViewMisSalaryCodeRepository.GetAllQueryable().AsQueryable(), Filter, nameof(ViewMisSalaryCode.SalaryAccountCode));
            return new FilterResult<SearchViewMisSalaryCodeModel>()
            {
                Data = _mapper.Map<List<SearchViewMisSalaryCodeModel>>(result.Data.ToList()),
                Total = result.Total

            };
        }


    }
}
