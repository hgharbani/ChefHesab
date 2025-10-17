using AutoMapper;
using KSC.Common;
using Ksc.HR.Appication.Interfaces.ODSViews;
using Ksc.HR.Appication.Interfaces.BusinessTrip;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Entities.BusinessTrip;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.ODSViews.ViewMisEmploymentType;
using Ksc.HR.DTO.BusinessTrip.Mission_TypeLocationWorkCity;
using KSC.Common.Filters.Contracts;
using KSC.Common.Filters.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkFlow.BaseFile;
using Ksc.HR.DTO.BusinessTrip.Mission_TypeLocation;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.Pay.OtherPaymentSettingParameterValue;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.DTO.WorkShift.RollCallWorkTimeMonthSetting;
using Ksc.HR.Share.General;

namespace Ksc.HR.Appication.Services.BusinessTrip
{
    public class RollCallWorkTimeMonthSettingService : IRollCallWorkTimeMonthSettingService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;
        private readonly IFilterHandler _FilterHandler;
        public RollCallWorkTimeMonthSettingService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper, IFilterHandler FilterHandler)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;

            _mapper = mapper;
            _FilterHandler = FilterHandler;
        }

        public FilterResult<SearchRollCallWorkTimeMonthSettingModel> GetRollCallWorkTimeMonthSettingByFilter(SearchRollCallWorkTimeMonthSettingModel Filter)
        {
            var query = _kscHrUnitOfWork.RollCallWorkTimeMonthSettingRepository.GetAllQueryable().Include(x => x.RollCallDefinition).Include(x => x.WorkTime).AsNoTracking();//
            if (Filter.RollCallDefinitionId != 0)
            {
                query = query.Where(x => x.RollCallDefinitionId == Filter.RollCallDefinitionId);
            }
            var data = query.ToList().Select(x => new SearchRollCallWorkTimeMonthSettingModel
            {
                Id = x.Id,
                RollCallDefinitionId = x.RollCallDefinitionId,
                RollCallDefinitionTitle = x.RollCallDefinition.Title,
                WorkTimeTitle = x.WorkTime.Title,
                IsActive = x.IsActive,
                DurationInMinute = x.DurationInMinute,
                Duration = Utility.ConvertMinuteIn24ToDuration(x.DurationInMinute)
            });
            var result = _FilterHandler.GetFilterResult<SearchRollCallWorkTimeMonthSettingModel>(data, Filter, nameof(RollCallWorkTimeMonthSetting.Id));

            var modelResult = new FilterResult<SearchRollCallWorkTimeMonthSettingModel>
            {
                Data = _mapper.Map<List<SearchRollCallWorkTimeMonthSettingModel>>(result.Data),
                Total = result.Total
            };
            return modelResult;

        }


    }
}
