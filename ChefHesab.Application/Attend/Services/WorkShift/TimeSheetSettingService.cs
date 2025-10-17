using AutoMapper;
using Ksc.HR.Appication.Interfaces.WorkShift;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.DTO.WorkShift.TimeSheetSetting;
using KSC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Services.WorkShift
{

    public class TimeSheetSettingService : ITimeSheetSettingService
    {
        private readonly IKscHrUnitOfWork _kscHrUnitOfWork;

        private readonly IMapper _mapper;

        public TimeSheetSettingService(IKscHrUnitOfWork kscHrUnitOfWork, IMapper mapper)
        {
            _kscHrUnitOfWork = kscHrUnitOfWork;
            _mapper = mapper;
        }

        public EditTimeSheetSettingModel GetForEdit()
        {
            var model = GetOne();
            return _mapper.Map<EditTimeSheetSettingModel>(model);
        }

        public TimeSheetSetting GetOne()
        {
            return _kscHrUnitOfWork.TimeSheetSettingRepository.FirstOrDefault(x => x.IsActive);
        }

        public async Task<KscResult> UpdateTimeSheetSetting(EditTimeSheetSettingModel model)
        {
            var result = model.IsValid();
            if (!result.Success)
                return result;
            var timeSheetSettingData = _mapper.Map<TimeSheetSetting>(model);
            //timeSheetSettingData.UpdateDate = DateTime.Now;
            //timeSheetSettingData.UpdateUser = model.UpdateUser;
            //timeSheetSettingData.DomainName = model.DomainName;
            //timeSheetSettingData.VacationEntitlementTimePerMonth = model.VacationEntitlementTimePerMonth;
            //timeSheetSettingData.BreastfeddingToleranceTime = model.BreastfeddingToleranceTime;
            //timeSheetSettingData.MinimumOverTimeAfterShiftInMinut = model.MinimumOverTimeAfterShiftInMinut;
            //timeSheetSettingData.MinimumShiftStartTimeInMinute = model.MinimumShiftStartTimeInMinute;
            //timeSheetSettingData.ForcedOverTimeBasic = model.ForcedOverTimeBasic;
            //timeSheetSettingData.WorkDayDuration = model.WorkDayDuration;
            //timeSheetSettingData.MinimumDailyVacation = model.MinimumDailyVacation;
            timeSheetSettingData.IsActive = true;
            await _kscHrUnitOfWork.SaveAsync();
            return result;
        }
    }
}
