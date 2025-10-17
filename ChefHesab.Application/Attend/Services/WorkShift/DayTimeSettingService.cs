using AutoMapper;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.DayTimeSetting;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.WorkShift
{
    public class DayTimeSettingService : IDayTimeSettingService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;

        public DayTimeSettingService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler filterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
            _FilterHandler = filterHandler;
        }

        public List<SearchDayTimeSettingModel> GetDayTimeSettingByKendoFilter(FilterRequest Filter)
        {
            var result = _FilterHandler.GetFilterResult<DayTimeSetting>(_kscHrUnitOfWork.DayTimeSettingRepository.GetQueryable(), Filter, "Id");
            return _mapper.Map<List<SearchDayTimeSettingModel>>(result.Data);
        }
    }
}
