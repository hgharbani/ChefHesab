using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;
using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using Ksc.HR.Share.Model.TimeShiftSetting;
using Ksc.HR.Share.Model.WorkCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.Define
{
    public interface IAttendanceRecordProvider
    {
        IQueryable<EmployeeAttendAbsenceItem> GetAttendanceRecords(int employeeId, int workCalendarId);
        bool HasExistingRecords(int employeeId, int workCalendarId);
    }

    public interface ITimeSettingCalculator
    {
        Task<TimeSettingDataModel> CalculateTimeSettings(TimeShiftDateTimeSettingModel model);
        Task<TimeSettingDataModel> GetShiftTimeSettingByDate(int employeeId, WorkCalendarForAttendAbcenseAnalysis workCalendar,
            List<TimeShiftSettingByWorkCityIdModel> timeShiftSettings, TimeShiftSettingByWorkCityIdModel currentSetting,
            int workGroupId, int? shiftConceptDetailId = null);
    }

    public interface IAttendanceAnalysisStrategy
    {
        bool CanHandle(EmployeeAttendAbsenceAnalysisInputModel input);
        Task<AnalysisAttenAbcenseResultModel> Analyze(EmployeeAttendAbsenceAnalysisInputModel input);
    }

    public interface IAnalysisResultBuilder
    {
        AnalysisAttenAbcenseResultModel BuildResult(IEnumerable<EmployeeAttendAbsenceAnalysisModel> items);
        void AddKarkardToResult(List<EmployeeAttendAbsenceAnalysisModel> result, AddKarkardToAnalysisViewModel model);
        void AddOverTimeToResult(List<EmployeeAttendAbsenceAnalysisModel> result, InputAddOverTimeToAnalysisModel model);
    }

    public interface IEntryExitProcessor
    {
        List<EmployeeEntryExitViewModel> ProcessEntryExits(List<EmployeeEntryExitViewModel> entries, TimeSettingDataModel timeSettings);
        void CheckOverlappingTimes(CheckOverlapppingTimeModel model);
    }
}
