using AutoMapper;
using KSC.Common;
using Ksc.Hr.Domain.Shared;
using Ksc.HR.Appication.Interfaces.WorkShift;

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.UnofficialHolidayReason;
using Ksc.HR.Resources.Messages;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class UnofficialHolidayReasonService : IUnofficialHolidayReasonService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public UnofficialHolidayReasonService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public List<SearchUnofficialHolidayReasonModel> GetUnofficialHolidayReasonsByKendoFilter(FilterRequest Filter)
        {
            var query = _kscHrUnitOfWork.UnofficialHolidayReasonRepository.WhereQueryable(a => a.IsActive).AsQueryable();
            var result = _FilterHandler.GetFilterResult<UnofficialHolidayReason>(query, Filter, "Id");
            var finalData = result.Data.Select(a => new SearchUnofficialHolidayReasonModel()
            {
                Id = a.Id,
                Title = a.Title,
                
            }).ToList();
            return _mapper.Map<List<SearchUnofficialHolidayReasonModel>>(finalData);
        }


    }
}
