using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.WorkCalendar;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface IWorkCalendarRepository : IRepository<WorkCalendar, int>
    {
        WorkCalendar GetByMiladiDate(DateTime dateTime);
        //WorkCalendar GetCurrentMonthStartInShamsi(DateTime dateTime);
        IQueryable<WorkCalendar> GetWorkCalendarByShamsiYear(int year);
        IQueryable<WorkCalendar> GetWorkCalendarIncluded();
        IQueryable<WorkCalendar> GetWorkCalendarQuerable();
        Task<int> GetDailyTimeSheetStatus(int workCalendarId);

        //Task<int> GetMonthTimeSheetStatus(int workCalendarId);
        Task<bool> GetMonthTimeSheetStatus(int workCalendarId);
        

        //WorkCalendar GetEndDayInCurrentMonth(DateTime dateTime);
        IQueryable<WorkCalendar> GetAllWorkCalendar(List<int> workCalendarId);
        IQueryable<WorkCalendar> GetAllQuerable();
        IQueryable<WorkCalendar> GetWorkCalendarByRangeDate(DateTime startDate, DateTime endDate);
        IQueryable<WorkCalendar> GetWorkCalendarByRangeDateAsNotracking(DateTime startDate, DateTime endDate);
        IQueryable<WorkCalendarForAttendAbcenseAnalysis> GetWorkCalendarForAttendAbcenseAnalysis(DateTime startDate, DateTime endDate, int? workCityId);
        IQueryable<WorkCalendar> GetWorkCalendarByYearMonthAsNoTracking(int yearMonth);
        (int, DateTime) GetYearMonthByWorkCalendarId(int workCalendarId);
        IQueryable<WorkCalendarForAttendAbcenseAnalysis> GetMonthWorkCalendar(int yearMonth);
        IQueryable<WorkCalendarForAttendAbcenseAnalysis> GetRangeMonthWorkCalendar(int startYearMonth, int endYearMonth);
        Task<bool> IsValidStatusForMission(int workCalendarId);
        IQueryable<WorkCalendar> GetIncludedWorkCalendarWorkDayType();
        IQueryable<WorkCalendar> GetAllAsNoTracking();
        (DateTime, DateTime) GetStartDateAndEndDateWithYearMonth(int yearMonth);
        string GetNextMonth(int yearMonth);
        string GetPrevMonth(int yearMonth);
        Task<bool> GetMonthTimeSheetStatus(int yearMonth, List<int?> systemSequenceStatusId);
        IQueryable<WorkCalendarForAttendAbcenseAnalysis> GetRangeDateKeyMonthWorkCalendar(int startDateKey, int endDateKey);
        (int, DateTime) GetNextYearMonthByShamsiYearMonth(int yearmonth);
        WorkCalendar GetByShamsiDate(string dateTime);
        (WorkCalendar, WorkCalendar) GetStartWorkCaldendarAndEndWorkCalendarWithYearMonth(int yearMonth);
        Task<bool> IsValidSystemControlDate(int yearMonth, int systemSequenceStatusId);
        WorkCalendar GetDateKeyMonthWorkCalendar(int DateKey);
        Task<bool> GetSystemSequenceStatusForStandbyBoard(int yearMonth);
        bool CheckValidMonthTimesheetYearMonth(int yearmonth);
        IQueryable<WorkCalendar> IsHoliday(DateTime date);
        int GetPrevYear(DateTime date);
        int GetNextYear(DateTime date);
        int GetLastPrevYearMonth(DateTime date);
        (int, DateTime, int?) GetYearMonthWorkDayTypeByWorkCalendarId(int workCalendarId);
        int AddMonthShamsi(int yearMonth, int monthToAdd);
        int CalculateMonthDifference(string date1, string date2);
    }
}
