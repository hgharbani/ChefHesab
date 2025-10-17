using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories.WorkShift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Model.TimeShiftSetting;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class TimeShiftSettingRepository : EfRepository<TimeShiftSetting, int>, ITimeShiftSettingRepository
    {
        private readonly KscHrContext _kscHrContext;
        public TimeShiftSettingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<TimeShiftSetting> GetAllByIncludedAsNotracking(DateTime date)
        {
            var query = _kscHrContext.TimeShiftSettings.Where(x => x.IsTemporaryTime == false && x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date)
                .Include(x => x.WorkCompanySetting).ThenInclude(x => x.WorkTimeShiftConcept).ThenInclude(x => x.ShiftConcept)
                .AsNoTracking();
            return query;
        }
        public async Task<TimeShiftSettingDataModel> GetShiftDateTimeSettingAsync(int employeeId, int shiftConceptDetailId, int workCityId, int workGroupId, int workCalendarId)
        {
            var workCalendar = await _kscHrContext.WorkCalendars.FirstOrDefaultAsync(x => x.Id == workCalendarId);
            DateTime date = workCalendar.MiladiDateV1;
            var workGroup = await _kscHrContext.WorkGroups.FirstOrDefaultAsync(x => x.Id == workGroupId);
            var workTime = await _kscHrContext.WorkTimes.FirstOrDefaultAsync(x => x.Id == workGroup.WorkTimeId);
            var queryshiftConceptDetail = _kscHrContext.ShiftConceptDetails.Include(x => x.ShiftConcept).ThenInclude(x => x.WorkTimeShiftConcepts).AsNoTracking();
            var shiftConceptDetail = await queryshiftConceptDetail.FirstAsync(x => x.Id == shiftConceptDetailId);
            var workTimeShiftConcept = shiftConceptDetail.ShiftConcept.WorkTimeShiftConcepts.First(x => x.WorkTimeId == workGroup.WorkTimeId);
            var workCompanySetting = await _kscHrContext.WorkCompanySettings.Include(x => x.TimeShiftSettings).AsNoTracking().FirstAsync(x => x.WorkCityId == workCityId && x.WorkTimeShiftConceptId == workTimeShiftConcept.Id);
            var timeShiftSetting = workCompanySetting.TimeShiftSettings.FirstOrDefault(x => x.IsTemporaryTime == false && x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date);
            if (timeShiftSetting == null)
            {
                return null;
            }
            var ShiftStartTime = timeShiftSetting.ShiftStartTime;
            var ShiftEndTime = timeShiftSetting.ShiftEndtTime;

            bool isRest = false;
            //
            var workDayType = await _kscHrContext.WorkDayTypes.FirstOrDefaultAsync(x => x.Id == workCalendar.WorkDayTypeId.Value);
            bool isOfficialHoliday = workDayType.IsHoliday && workDayType.IsOfficialHoliday;
            bool isUnOfficialHoliday = workDayType.IsHoliday && !workDayType.IsOfficialHoliday;
            bool isHoliday = workDayType.IsHoliday;
            //
            //if (workTime.ShiftSettingFromShiftboard)
            //{
            //    isRest = shiftConceptDetail.ShiftConcept.IsRest;
            //    if (workTime.OfficialUnOfficialHolidayFromWorkCalendar && workDayType.IsHoliday) //درصورتیکه تعطیلات رسمی-غیر رسمی از روی تقویم باشد
            //        isRest = true;
            //}
            //else
            //{
            //    var shiftHoliday = await _kscHrContext.ShiftHolidays.FirstOrDefaultAsync(x => x.WorkCompanySettingId == workCompanySetting.Id && x.DayNumber == workCalendar.DayOfWeek);
            //    isRest = shiftHoliday != null || workDayType.IsHoliday;
            //}
            isRest = await IsRestShiftAsync(workTime.ShiftSettingFromShiftboard, workTime.OfficialUnOfficialHolidayFromWorkCalendar, shiftConceptDetail.ShiftConcept.IsRest, workDayType.IsHoliday, workCompanySetting.Id, workCalendar.DayOfWeek);

            //
            //
            var ShiftEndDate = date;
            if (ShiftStartTime.ConvertStringToTimeSpan() > ShiftEndTime.ConvertStringToTimeSpan())
            {
                ShiftEndDate = ShiftEndDate.AddDays(1);
            }
            var timeSheetSetting = await _kscHrContext.TimeSheetSettings.SingleOrDefaultAsync(x => x.IsActive);
            TimeShiftSettingDataModel model = new TimeShiftSettingDataModel()
            {
                ShiftStartTime = ShiftStartTime,
                ShiftStartDate = date,
                ShiftEndTime = ShiftEndTime,
                ShiftEndDate = ShiftEndDate,
                IsRestShift = isRest,
                MinimumOverTimeAfterShiftInMinut = timeSheetSetting.MinimumOverTimeAfterShiftInMinut,
                ShiftSettingFromShiftboard = workTime.ShiftSettingFromShiftboard,
                MinimumWorkHourInDay = timeShiftSetting.MinimumWorkHourInDay,
                TotalWorkHourInDay = timeShiftSetting.TotalWorkHourInDay
            };
            return model;
        }

        public TimeShiftSettingDataModel GetShiftDateTimeSetting(int employeeId, int shiftConceptDetailId, int workCityId, int workGroupId, int workCalendarId)
        {
            var workCalendar = _kscHrContext.WorkCalendars.FirstOrDefault(x => x.Id == workCalendarId);
            DateTime date = workCalendar.MiladiDateV1;
            var workGroup = _kscHrContext.WorkGroups.FirstOrDefault(x => x.Id == workGroupId);
            var workTime = _kscHrContext.WorkTimes.FirstOrDefault(x => x.Id == workGroup.WorkTimeId);
            var queryshiftConceptDetail = _kscHrContext.ShiftConceptDetails.Include(x => x.ShiftConcept).ThenInclude(x => x.WorkTimeShiftConcepts).AsNoTracking();
            var shiftConceptDetail = queryshiftConceptDetail.First(x => x.Id == shiftConceptDetailId);
            var workTimeShiftConcept = shiftConceptDetail.ShiftConcept.WorkTimeShiftConcepts.First(x => x.WorkTimeId == workGroup.WorkTimeId);
            var workCompanySetting = _kscHrContext.WorkCompanySettings.Include(x => x.TimeShiftSettings).AsNoTracking().First(x => x.WorkCityId == workCityId && x.WorkTimeShiftConceptId == workTimeShiftConcept.Id);
            var timeShiftSetting = workCompanySetting.TimeShiftSettings.FirstOrDefault(x => x.IsTemporaryTime == false && x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date);
            if (timeShiftSetting == null)
            {
                return null;
            }
            //
            var minimumWorkHourInDay = timeShiftSetting.MinimumWorkHourInDay;
            var timeShiftSettingTemporary = workCompanySetting.TimeShiftSettings.FirstOrDefault(x => x.IsTemporaryTime && x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date);
            //
            if (timeShiftSettingTemporary != null && timeShiftSettingTemporary.MinimumWorkHourInDay != null)
            {
                minimumWorkHourInDay = timeShiftSettingTemporary.MinimumWorkHourInDay;
            }
            //
            var ShiftStartTime = timeShiftSetting.ShiftStartTime;
            var ShiftEndTime = timeShiftSetting.ShiftEndtTime;

            bool isRest = false;
            //
            var workDayType = _kscHrContext.WorkDayTypes.FirstOrDefault(x => x.Id == workCalendar.WorkDayTypeId.Value);
            bool isOfficialHoliday = workDayType.IsHoliday && workDayType.IsOfficialHoliday;
            bool isUnOfficialHoliday = workDayType.IsHoliday && !workDayType.IsOfficialHoliday;
            bool isHoliday = workDayType.IsHoliday;
            //
            //if (workTime.ShiftSettingFromShiftboard)
            //{
            //    isRest = shiftConceptDetail.ShiftConcept.IsRest;
            //    if (workTime.OfficialUnOfficialHolidayFromWorkCalendar && workDayType.IsHoliday) //درصورتیکه تعطیلات رسمی-غیر رسمی از روی تقویم باشد
            //        isRest = true;
            //}
            //else
            //{
            //    var shiftHoliday = _kscHrContext.ShiftHolidays.FirstOrDefault(x => x.WorkCompanySettingId == workCompanySetting.Id && x.DayNumber == workCalendar.DayOfWeek);
            //    isRest = shiftHoliday != null || workDayType.IsHoliday;
            //}
            isRest = IsRestShift(workTime.ShiftSettingFromShiftboard, workTime.OfficialUnOfficialHolidayFromWorkCalendar, shiftConceptDetail.ShiftConcept.IsRest, workDayType.IsHoliday, workCompanySetting.Id, workCalendar.DayOfWeek);
            //
            //
            var ShiftEndDate = date;
            if (ShiftStartTime.ConvertStringToTimeSpan() > ShiftEndTime.ConvertStringToTimeSpan())
            {
                ShiftEndDate = ShiftEndDate.AddDays(1);
            }
            var timeSheetSetting = _kscHrContext.TimeSheetSettings.SingleOrDefault(x => x.IsActive);
            //

            //
            TimeShiftSettingDataModel model = new TimeShiftSettingDataModel()
            {
                ShiftStartTime = ShiftStartTime,
                ShiftStartDate = date,
                ShiftEndTime = ShiftEndTime,
                ShiftEndDate = ShiftEndDate,
                IsRestShift = isRest,
                MinimumOverTimeAfterShiftInMinut = timeSheetSetting.MinimumOverTimeAfterShiftInMinut,
                ShiftSettingFromShiftboard = workTime.ShiftSettingFromShiftboard,
                MinimumWorkHourInDay = minimumWorkHourInDay//timeShiftSetting.MinimumWorkHourInDay
            };
            return model;
        }
        /// <summary>
        /// گرفتن روز استراحت با توجه به تعطیلی در تقویم-تعطیلی در شیفت
        /// </summary>
        /// <param name="shiftSettingFromShiftboard"></param> تنظیمات شیفت از لوحه شیفت باشد یا تقویم کاری
        /// <param name="officialUnOfficialHolidayFromWorkCalendar"></param> تعطیلات رسمی-غیر رسمی از تقویم کاری باشد
        /// <param name="shiftConceptIsRest"></param> از نوع شیفت استراحت است؟
        /// <param name="workDayTypeIsHoliday"></param> نوع روز تعطیلی است؟
        /// <param name="workCompanySettingId"></param> 
        /// <param name="dayNumber"></param>شماره روز
        /// <returns></returns>
        public async Task<bool> IsRestShiftAsync(bool shiftSettingFromShiftboard, bool officialUnOfficialHolidayFromWorkCalendar
            , bool shiftConceptIsRest, bool workDayTypeIsHoliday, int workCompanySettingId, int dayNumber)
        {
            bool isRest = false;
            bool isShiftHoliday = false;
            if (shiftSettingFromShiftboard == false)
            {
                isShiftHoliday = await _kscHrContext.ShiftHolidays.AnyAsync(x => x.WorkCompanySettingId == workCompanySettingId && x.DayNumber == dayNumber);
            }
            isRest = Utility.IsRestShift(shiftSettingFromShiftboard, officialUnOfficialHolidayFromWorkCalendar, shiftConceptIsRest, workDayTypeIsHoliday, isShiftHoliday);
            return isRest;
        }
        /// <summary>
        /// گرفتن روز استراحت با توجه به تعطیلی در تقویم-تعطیلی در شیفت
        /// </summary>
        /// <param name="shiftSettingFromShiftboard"></param> تنظیمات شیفت از لوحه شیفت باشد یا تقویم کاری
        /// <param name="officialUnOfficialHolidayFromWorkCalendar"></param> تعطیلات رسمی-غیر رسمی از تقویم کاری باشد
        /// <param name="shiftConceptIsRest"></param> از نوع شیفت استراحت است؟
        /// <param name="workDayTypeIsHoliday"></param> نوع روز تعطیلی است؟
        /// <param name="workCompanySettingId"></param> 
        /// <param name="DayNumber"></param>شماره روز
        /// <returns></returns>
        public bool IsRestShift(bool shiftSettingFromShiftboard, bool officialUnOfficialHolidayFromWorkCalendar
            , bool shiftConceptIsRest, bool workDayTypeIsHoliday, int workCompanySettingId, int dayNumber)
        {
            bool isRest = false;
            bool isShiftHoliday = false;
            if (shiftSettingFromShiftboard == false)
            {
                isShiftHoliday = _kscHrContext.ShiftHolidays.Any(x => x.WorkCompanySettingId == workCompanySettingId && x.DayNumber == dayNumber);
            }
            isRest = Utility.IsRestShift(shiftSettingFromShiftboard, officialUnOfficialHolidayFromWorkCalendar, shiftConceptIsRest, workDayTypeIsHoliday, isShiftHoliday);
            return isRest;
        }

        public IQueryable<TimeShiftSettingByWorkCityIdModel> GetTimeShiftSettingByWorkCityId(int workCityId)
        {
            var data = from time in _kscHrContext.TimeShiftSettings
                       join company in _kscHrContext.WorkCompanySettings on time.WorkCompanySettingId equals company.Id
                       join timeshift in _kscHrContext.WorkTimeShiftConcepts on company.WorkTimeShiftConceptId equals timeshift.Id
                       join worktime in _kscHrContext.WorkTimes on timeshift.WorkTimeId equals worktime.Id
                       join shiftConcept in _kscHrContext.ShiftConcepts on timeshift.ShiftConceptId equals shiftConcept.Id
                       where company.WorkCityId == workCityId && time.IsActive
                       select new TimeShiftSettingByWorkCityIdModel()
                       {
                           TimeShiftSettingId = time.Id,
                           WorkCompanySettingId = time.WorkCompanySettingId,
                           ValidityStartDate = time.ValidityStartDate,
                           ValidityEndDate = time.ValidityEndDate,
                           ShiftStartTime = time.ShiftStartTime,
                           ShiftEndtTime = time.ShiftEndtTime,
                           ToleranceShiftStartTime = time.ToleranceShiftStartTime,
                           ToleranceShiftEndTime = time.ToleranceShiftEndTime,
                           PercentageWorkTime = time.PercentageWorkTime,
                           TotalWorkHourInDay = time.TotalWorkHourInDay,
                           IsTemporaryTime = time.IsTemporaryTime,
                           TemporaryRollCallDefinitionStartShift = time.TemporaryRollCallDefinitionStartShift,
                           TemporaryRollCallDefinitionEndShift = time.TemporaryRollCallDefinitionEndShift,
                           WorktimeId = worktime.Id,
                           ShiftSettingFromShiftboard = worktime.ShiftSettingFromShiftboard,
                           OfficialUnOfficialHolidayFromWorkCalendar = worktime.OfficialUnOfficialHolidayFromWorkCalendar,
                           ShiftConceptId = timeshift.ShiftConceptId,
                           ShiftConceptIsRest = shiftConcept.IsRest,
                           ForcedOverTime = time.ForcedOverTime != null ? time.ForcedOverTime.ConvertDurationToMinute().Value : 0,
                           TotalWorkHourInWeek = time.TotalWorkHourInWeek,
                           TemprorayOverTimeRollCallDefinitionEndShift = time.TemprorayOverTimeRollCallDefinitionEndShift,
                           TemprorayOverTimeRollCallDefinitionStartShift = time.TemprorayOverTimeRollCallDefinitionStartShift,
                           BreastfeddingToleranceTime = time.BreastfeddingToleranceTime,
                           TemprorayOverTimeDurationInEndShift = time.TemprorayOverTimeDuration,
                           TemprorayOverTimeDurationInStartShift = time.TemprorayOverTimeDurationInStartShift,
                           CheckedEmployeeValidOverTime = time.CheckedEmployeeValidOverTime,
                           ConditionalAbsenceToleranceTime = time.ConditionalAbsenceToleranceTime,
                           ValidOverTimeStartTime = time.ValidOverTimeStartTime,
                           MinimumWorkHourInDay=time.MinimumWorkHourInDay

                       };
            return data;
        }

        public IQueryable<TimeShiftSettingByWorkCityIdModel> GetTimeShiftSettingAsNoTracking()
        {
            var data = (from time in _kscHrContext.TimeShiftSettings
                        join company in _kscHrContext.WorkCompanySettings on time.WorkCompanySettingId equals company.Id
                        join timeshift in _kscHrContext.WorkTimeShiftConcepts on company.WorkTimeShiftConceptId equals timeshift.Id
                        join worktime in _kscHrContext.WorkTimes on timeshift.WorkTimeId equals worktime.Id
                        join shiftConcept in _kscHrContext.ShiftConcepts on timeshift.ShiftConceptId equals shiftConcept.Id
                        where time.IsActive
                        select new TimeShiftSettingByWorkCityIdModel()
                        {
                            TimeShiftSettingId = time.Id,
                            WorkCompanySettingId = time.WorkCompanySettingId,
                            ValidityStartDate = time.ValidityStartDate,
                            ValidityEndDate = time.ValidityEndDate,
                            ShiftStartTime = time.ShiftStartTime,
                            ShiftEndtTime = time.ShiftEndtTime,
                            ToleranceShiftStartTime = time.ToleranceShiftStartTime,
                            ToleranceShiftEndTime = time.ToleranceShiftEndTime,
                            PercentageWorkTime = time.PercentageWorkTime,
                            TotalWorkHourInDay = time.TotalWorkHourInDay,
                            IsTemporaryTime = time.IsTemporaryTime,
                            TemporaryRollCallDefinitionStartShift = time.TemporaryRollCallDefinitionStartShift,
                            TemporaryRollCallDefinitionEndShift = time.TemporaryRollCallDefinitionEndShift,
                            WorktimeId = worktime.Id,
                            ShiftSettingFromShiftboard = worktime.ShiftSettingFromShiftboard,
                            OfficialUnOfficialHolidayFromWorkCalendar = worktime.OfficialUnOfficialHolidayFromWorkCalendar,
                            ShiftConceptId = timeshift.ShiftConceptId,
                            ShiftConceptIsRest = shiftConcept.IsRest,
                            ForcedOverTime = time.ForcedOverTime != null ? time.ForcedOverTime.ConvertDurationToMinute().Value : 0,
                            TotalWorkHourInWeek = time.TotalWorkHourInWeek,
                            WorkCityId = company.WorkCityId

                        }).AsNoTracking();
            return data;
        }

        public IQueryable<TimeShiftSetting> GetTemporaryTimeByIncludedAsNotracking(DateTime date)
        {
            var query = _kscHrContext.TimeShiftSettings.Where(x => x.IsTemporaryTime == true && x.ValidityStartDate.Value.Date <= date.Date && x.ValidityEndDate.Value.Date >= date.Date)
                .Include(x => x.WorkCompanySetting).ThenInclude(x => x.WorkTimeShiftConcept).ThenInclude(x => x.ShiftConcept)
                .AsNoTracking();
            return query;
        }
    }
}


