using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.TimeShiftSetting;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface ITimeShiftSettingRepository : IRepository<TimeShiftSetting, int>
    {
        IQueryable<TimeShiftSetting> GetAllByIncludedAsNotracking(DateTime date);
        Task<TimeShiftSettingDataModel> GetShiftDateTimeSettingAsync(int employeeId, int shiftConceptDetailId, int workCityId, int workGroupId, int workCalendarId);
        TimeShiftSettingDataModel GetShiftDateTimeSetting(int employeeId, int shiftConceptDetailId, int workCityId, int workGroupId, int workCalendarId);
        Task<bool> IsRestShiftAsync(bool shiftSettingFromShiftboard, bool officialUnOfficialHolidayFromWorkCalendar, bool shiftConceptIsRest, bool workDayTypeIsHoliday, int workCompanySettingId, int dayNumber);
        bool IsRestShift(bool shiftSettingFromShiftboard, bool officialUnOfficialHolidayFromWorkCalendar, bool ShiftConceptIsRest, bool workDayTypeIsHoliday, int workCompanySettingId, int dayNumber);
        IQueryable<TimeShiftSettingByWorkCityIdModel> GetTimeShiftSettingByWorkCityId(int workCityId);
        IQueryable<TimeShiftSettingByWorkCityIdModel> GetTimeShiftSettingAsNoTracking();
        IQueryable<TimeShiftSetting> GetTemporaryTimeByIncludedAsNotracking(DateTime date);
    }
}
