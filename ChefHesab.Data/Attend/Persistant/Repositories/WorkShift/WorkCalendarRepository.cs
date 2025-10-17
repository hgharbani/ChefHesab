using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;
using DNTPersianUtils.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.SystemSequenceStatus;
using Ksc.HR.Share.Model.WorkCalendar;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Entities.HRSystemStatusControl;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class WorkCalendarRepository : EfRepository<WorkCalendar, int>, IWorkCalendarRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WorkCalendarRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<WorkCalendar> GetAllQuerable()
        {
            var query = _kscHrContext.WorkCalendars.AsQueryable();
            return query;
        }
        public IQueryable<WorkCalendar> GetAllAsNoTracking()
        {
            var query = GetAllQuerable().AsNoTracking();
            return query;
        }
        public IQueryable<WorkCalendar> GetWorkCalendarQuerable()
        {
            var query = _kscHrContext.WorkCalendars.Include(a => a.EmployeeAttendAbsenceItems).AsQueryable();
            return query;
        }
        public IQueryable<WorkCalendar> GetIncludedWorkCalendarWorkDayType()
        {
            var query = _kscHrContext.WorkCalendars.Include(a => a.WorkDayType).AsQueryable();
            return query;
        }
        public IQueryable<WorkCalendar> GetWorkCalendarByShamsiYear(int year)
        {
            var query = _kscHrContext.WorkCalendars.Where(x => x.YyyyShamsi == year);
            return query;
        }

        public WorkCalendar GetByMiladiDate(DateTime dateTime)
        {
            //var query = GetAll().First(x => x.MiladiDateV1 == dateTime.Date);
            var query = _kscHrContext.WorkCalendars.Include(x => x.WorkDayType).First(x => x.MiladiDateV1 == dateTime.Date);

            return query;
        }
        public IQueryable<WorkCalendar> GetWorkCalendarIncluded()
        {
            var query = _kscHrContext.WorkCalendars.Include(a => a.WorkDayType).Include(x => x.SystemSequenceStatus).AsQueryable().AsNoTracking();
            return query;
        }

        public IQueryable<WorkCalendar> GetAllWorkCalendar(List<int> workCalendarId)
        {
            var query = _kscHrContext.WorkCalendars.Where(a => workCalendarId.Contains(a.Id)).AsQueryable().AsNoTracking();
            return query;
        }
        public async Task<int> GetDailyTimeSheetStatus(int workCalendarId)
        {
            var workCalendar = await GetByIdAsync(workCalendarId);

            var systemControlDate = _kscHrContext.SystemControlDates.FirstOrDefault();
            if (systemControlDate.AttendAbsenceItemDate > workCalendar.YearMonthV1)
            {
                // if (workCalendar.SystemSequenceStatusId == null || workCalendar.SystemSequenceStatusId.Value != EnumSystemSequenceStatusDailyTimeSheet.TimeSheetIsCreated.Id)
                return EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id;
            }
            return workCalendar.SystemSequenceStatusId.Value;
        }

        //public async Task<int> GetMonthTimeSheetStatus(int workCalendarId)
        /// <summary>
        /// بررسی میکند در سال ماه ورودی به تابع روزهای ماه برای تمام کاربران بسته شده است یا خیر؟
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public async Task<bool> GetMonthTimeSheetStatus(int yearMonth)
        {
            var workCalendar = _kscHrContext.WorkCalendars.Where(x => x.YearMonthV1 == yearMonth);


            var systemControlDate = await _kscHrContext.SystemControlDates.FirstOrDefaultAsync();
            if (systemControlDate.AttendAbsenceItemDate == yearMonth && workCalendar.All(x => x.SystemSequenceStatusId == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id))
            {
                //return EnumSystemSequenceStatusDailyTimeSheet.InActiveSystem.Id;
                return true;
            }
            //return workCalendar.SystemSequenceStatusId.Value;
            return false;
        }

        //public async Task<int> GetMonthTimeSheetStatus(int workCalendarId)
        /// <summary>
        /// بررسی میکند در سال ماه ورودی به تابع روزهای ماه برای تمام کاربران بسته شده است یا خیر؟
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public async Task<bool> GetMonthTimeSheetStatus(int yearMonth, List<int?> systemSequenceStatusId)
        {
            var workCalendar = _kscHrContext.WorkCalendars.Where(x => x.YearMonthV1 == yearMonth);


            var systemControlDate = await _kscHrContext.SystemControlDates.FirstOrDefaultAsync();
            if (systemSequenceStatusId.Any())
            {
                if (systemControlDate.AttendAbsenceItemDate == yearMonth && workCalendar.All(x => systemSequenceStatusId.Contains(x.SystemSequenceStatusId)))
                {
                    //return EnumSystemSequenceStatusDailyTimeSheet.InActiveSystem.Id;
                    return true;
                }
            }
            else
            {
                if (systemControlDate.AttendAbsenceItemDate == yearMonth && workCalendar.All(x => x.SystemSequenceStatusId == EnumSystemSequenceStatusDailyTimeSheet.InActiveForAllUser.Id))
                {
                    //return EnumSystemSequenceStatusDailyTimeSheet.InActiveSystem.Id;
                    return true;
                }
            }

            //return workCalendar.SystemSequenceStatusId.Value;
            return false;
        }

        public WorkCalendar GetByShamsiDate(string dateTime)
        {
            var query = GetAll().AsQueryable().First(x => x.ShamsiDateV1 == dateTime);
            return query;
        }
        ////تاریخ اول ماه
        //public WorkCalendar GetCurrentMonthStartInShamsi(DateTime dateTime)
        //{
        //    var StartcurrentMonth = "";
        //    var workCalendar = GetByMiladiDate(dateTime); //workCalendar
        //    if (workCalendar != null)
        //        StartcurrentMonth = workCalendar.YearMonthV2.ToString() + "/01";

        //    var result = GetByShamsiDate(StartcurrentMonth);
        //    return result;

        //}
        ///// <summary>
        ///// رکورد روز آخر ماه
        ///// </summary>
        ///// <param name="dateTime"></param>
        ///// <returns></returns>
        //public WorkCalendar GetEndDayInCurrentMonth(DateTime dateTime)
        //{
        //    var workCalendar = GetByMiladiDate(dateTime); //workCalendar //YYYYMM
        //    var DaysInMonth = _kscHrContext.WorkCalendars.Count(x => x.YearMonthV1 == workCalendar.YearMonthV1);//DD
        //    var EndcurrentMonth = workCalendar.YearMonthV2.ToString()+"/" + DaysInMonth.ToString("00");
        //    var result = GetByShamsiDate(EndcurrentMonth);
        //    return result;

        //}
        public IQueryable<WorkCalendar> GetWorkCalendarByRangeDate(DateTime startDate, DateTime endDate)
        {
            return _kscHrContext.WorkCalendars.Where(x => x.MiladiDateV1 >= startDate && x.MiladiDateV1 <= endDate).Include(x => x.WorkDayType);
        }
        public IQueryable<WorkCalendar> GetWorkCalendarByRangeDateAsNotracking(DateTime startDate, DateTime endDate)
        {
            return _kscHrContext.WorkCalendars.Where(x => x.MiladiDateV1 >= startDate && x.MiladiDateV1 <= endDate).AsNoTracking();
        }
        public IQueryable<WorkCalendarForAttendAbcenseAnalysis> GetWorkCalendarForAttendAbcenseAnalysis(DateTime startDate, DateTime endDate, int? workCityId)
        {
            var workCalendar = _kscHrContext.WorkCalendars.Where(x => x.MiladiDateV1 >= startDate && x.MiladiDateV1 <= endDate).Include(x => x.WorkDayType).AsNoTracking();
            var workCalendars = workCalendar.Select(x => new WorkCalendarForAttendAbcenseAnalysis()
            {
                WorkCalendarId = x.Id,
                IsOfficialHoliday = x.WorkDayType.IsHoliday && x.WorkDayType.IsOfficialHoliday,
                IsUnOfficialHoliday = x.WorkDayType.IsHoliday && !x.WorkDayType.IsOfficialHoliday,
                IsHoliday = x.WorkDayType.IsHoliday,
                DayNumber = x.DayOfWeek,
                WorkDayTypeId = x.WorkDayTypeId.Value,
                Date = x.MiladiDateV1,
                YearMonth = x.YearMonthV1,
                DateKey = x.DateKey
            });
            return workCalendars;
        }
        public ValueTuple<DateTime, DateTime> GetStartEndMonth(int yearMonth)
        {
            var result = _kscHrContext.WorkCalendars.Where(x => x.YearMonthV1 == yearMonth).Select(x => x.MiladiDateV1).ToList();
            return new ValueTuple<DateTime, DateTime>(result.Min(), result.Max());
        }
        public IQueryable<WorkCalendar> GetWorkCalendarByYearMonthAsNoTracking(int yearMonth)
        {
            var result = _kscHrContext.WorkCalendars.Where(x => x.YearMonthV1 == yearMonth).AsNoTracking();
            return result;
        }
        public ValueTuple<int, DateTime> GetYearMonthByWorkCalendarId(int workCalendarId)
        {
            var query = _kscHrContext.WorkCalendars.Where(x => x.Id == workCalendarId).Select(x => new { YearMonth = x.YearMonthV1, MiladiDate = x.MiladiDateV1 }).First();
            return new ValueTuple<int, DateTime>(query.YearMonth, query.MiladiDate);
        }
        public ValueTuple<int, DateTime, int?> GetYearMonthWorkDayTypeByWorkCalendarId(int workCalendarId)
        {
            var query = _kscHrContext.WorkCalendars.Where(x => x.Id == workCalendarId).AsNoTracking().Select(x => new { YearMonth = x.YearMonthV1, MiladiDate = x.MiladiDateV1, WorkDayTypeId = x.WorkDayTypeId }).First();
            return new ValueTuple<int, DateTime, int?>(query.YearMonth, query.MiladiDate, query.WorkDayTypeId);
        }

        public ValueTuple<int, DateTime> GetNextYearMonthByShamsiYearMonth(int yearmonth)
        {
            var nextdate = GetNextMonth(yearmonth);
            var query = _kscHrContext.WorkCalendars.Where(x => x.ShamsiDateV1 == nextdate).Select(x => new { YearMonth = x.YearMonthV1, MiladiDate = x.MiladiDateV1 }).First();
            return new ValueTuple<int, DateTime>(query.YearMonth, query.MiladiDate);
        }
        public ValueTuple<DateTime, DateTime> GetStartDateAndEndDateWithYearMonth(int yearMonth)
        {
            var query = _kscHrContext.WorkCalendars.Where(x => x.YearMonthV1 == yearMonth).AsNoTracking().Select(a => a.MiladiDateV1).ToList();
            var startDate = query.OrderBy(a => a).First();
            var endDate = query.OrderByDescending(a => a).First();
            return new ValueTuple<DateTime, DateTime>(startDate, endDate);
        }
        public ValueTuple<WorkCalendar, WorkCalendar> GetStartWorkCaldendarAndEndWorkCalendarWithYearMonth(int yearMonth)
        {
            var query = _kscHrContext.WorkCalendars.Where(x => x.YearMonthV1 == yearMonth).AsNoTracking().ToList();
            var startDate = query.OrderBy(a => a.MiladiDateV1).First();
            var endDate = query.OrderByDescending(a => a.MiladiDateV1).First();
            return new ValueTuple<WorkCalendar, WorkCalendar>(startDate, endDate);
        }




        public IQueryable<WorkCalendarForAttendAbcenseAnalysis> GetMonthWorkCalendar(int yearMonth)
        {
            var workCalendar = _kscHrContext.WorkCalendars.Where(x => x.YearMonthV1 == yearMonth).Include(x => x.WorkDayType).AsNoTracking();
            var workCalendars = workCalendar.Select(x => new WorkCalendarForAttendAbcenseAnalysis()
            {
                WorkCalendarId = x.Id,
                IsOfficialHoliday = x.WorkDayType.IsHoliday && x.WorkDayType.IsOfficialHoliday,
                DayNumber = x.DayOfWeek,
                WorkDayTypeId = x.WorkDayTypeId.Value,
                Date = x.MiladiDateV1,
                YearMonth = x.YearMonthV1
            });
            return workCalendars;
        }
        public IQueryable<WorkCalendarForAttendAbcenseAnalysis> GetRangeMonthWorkCalendar(int startYearMonth, int endYearMonth)
        {
            var workCalendar = _kscHrContext.WorkCalendars.Where(x => x.YearMonthV1 >= startYearMonth && x.YearMonthV1 <= endYearMonth).Include(x => x.WorkDayType).AsNoTracking();
            var workCalendars = workCalendar.Select(x => new WorkCalendarForAttendAbcenseAnalysis()
            {
                WorkCalendarId = x.Id,
                IsOfficialHoliday = x.WorkDayType.IsHoliday && x.WorkDayType.IsOfficialHoliday,
                DayNumber = x.DayOfWeek,
                WorkDayTypeId = x.WorkDayTypeId.Value,
                Date = x.MiladiDateV1,
                YearMonth = x.YearMonthV1
            });
            return workCalendars;
        }

        public async Task<bool> IsValidStatusForMission(int workCalendarId)
        {
            var workCalendar = await GetByIdAsync(workCalendarId);

            var systemControlDate = _kscHrContext.SystemControlDates.FirstOrDefault();
            if (systemControlDate.AttendAbsenceItemDate > workCalendar.YearMonthV1) // تاریخ قبل از تاریخ کارکرد سیستم
            {
                return false;
            }
            if (workCalendar.YearMonthV1 > systemControlDate.AttendAbsenceItemDate)// تاریخ بعد از تاریخ کارکرد سیستم
            {
                return true;
            }
            // تاریخ برابر با تاریخ کارکرد سیستم
            if (workCalendar.SystemSequenceStatusId.Value == EnumSystemSequenceStatusDailyTimeSheet.ActiveForAllUser.Id ||
                workCalendar.SystemSequenceStatusId.Value == EnumSystemSequenceStatusDailyTimeSheet.ActiveForOfficialUser.Id)
            {
                return true;
            }
            return false;


        }

        public string GetNextMonth(int yearMonth)
        {
            var yearMonth_str = yearMonth.ToString();
            var persianDate = int.Parse(yearMonth_str.Substring(0, 4)).ToPersianDate(int.Parse(yearMonth_str.Substring(4, 2)), 1);
            var nexmonth = persianDate.GetPersianMonthStartAndEndDates().EndDate.AddDays(1).ToPersianYearMonthDay();
            return nexmonth.ToString();

        }
        public string GetPrevMonth(int yearMonth)
        {
            var yearMonth_str = yearMonth.ToString();
            var persianDate = int.Parse(yearMonth_str.Substring(0, 4)).ToPersianDate(int.Parse(yearMonth_str.Substring(4, 2)), 1);
            var nexmonth = persianDate.GetPersianMonthStartAndEndDates().StartDate.AddDays(-1).GetPersianMonthStartAndEndDates().StartDate.ToPersianYearMonthDay();
            return nexmonth.ToString();

        }

        public int GetPrevYear(DateTime date)
        {
            var findwork = _kscHrContext.WorkCalendars.FirstOrDefault(a => a.MiladiDateV1 == date.Date);
            if (findwork != null)
            {

                var prevYear = int.Parse(findwork.YearMonthV1.ToString().Substring(0, 4)) - 1;
                return prevYear;
            }
            return 0;
        }

        public int GetLastPrevYearMonth(DateTime date)
        {
            var findwork = _kscHrContext.WorkCalendars.FirstOrDefault(a => a.MiladiDateV1 == date.Date);
            if (findwork != null)
            {

                var prevYear = (int.Parse(findwork.YearMonthV1.ToString().Substring(0, 4)) - 1).ToString() + "12";


                return int.Parse(prevYear);


            }
            return 0;
        }
        public int GetNextYear(DateTime date)
        {
            var findwork = _kscHrContext.WorkCalendars.FirstOrDefault(a => a.MiladiDateV1 == date.Date);
            if (findwork != null)
            {

                var prevYear = int.Parse(findwork.YearMonthV1.ToString().Substring(0, 4)) + 1;
                return prevYear;
            }
            return 0;
        }
        public IQueryable<WorkCalendarForAttendAbcenseAnalysis> GetRangeDateKeyMonthWorkCalendar(int startDateKey, int endDateKey)
        {
            var workCalendar = _kscHrContext.WorkCalendars.Where(x => x.DateKey >= startDateKey && x.DateKey <= endDateKey).Include(x => x.WorkDayType).AsNoTracking();
            var workCalendars = workCalendar.Select(x => new WorkCalendarForAttendAbcenseAnalysis()
            {
                WorkCalendarId = x.Id,
                IsOfficialHoliday = x.WorkDayType.IsHoliday && x.WorkDayType.IsOfficialHoliday,
                DayNumber = x.DayOfWeek,
                WorkDayTypeId = x.WorkDayTypeId.Value,
                Date = x.MiladiDateV1,
                YearMonth = x.YearMonthV1,
                DateKey = x.DateKey,
                IsHoliday = x.WorkDayType.IsHoliday

            });
            return workCalendars;
        }
        public WorkCalendar GetDateKeyMonthWorkCalendar(int DateKey)
        {
            var workCalendar = _kscHrContext.WorkCalendars.FirstOrDefault(x => x.DateKey == DateKey);

            return workCalendar;
        }
        public async Task<bool> IsValidSystemControlDate(int yearMonth, int systemSequenceStatusId)
        {
            var workCalendar = _kscHrContext.WorkCalendars.Select(x => new { YearMonthV1 = x.YearMonthV1, SystemSequenceStatusId = x.SystemSequenceStatusId })
                .Where(x => x.YearMonthV1 == yearMonth);


            var systemControlDate = await _kscHrContext.SystemControlDates.FirstOrDefaultAsync();
            if (systemControlDate.AttendAbsenceItemDate == yearMonth && workCalendar.All(x => x.SystemSequenceStatusId == systemSequenceStatusId))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> GetSystemSequenceStatusForStandbyBoard(int yearMonth)
        {
            var systemControlDate = await _kscHrContext.SystemControlDates.FirstOrDefaultAsync();
            if (yearMonth > systemControlDate.AttendAbsenceItemDate)
                return true;
            else if (systemControlDate.AttendAbsenceItemDate == yearMonth)
            {

                var workCalendar = _kscHrContext.WorkCalendars.Where(x => x.YearMonthV1 == yearMonth);
                if (workCalendar.All(x => x.SystemSequenceStatusId == EnumSystemSequenceStatusDailyTimeSheet.ActiveForAllUser.Id || x.SystemSequenceStatusId == EnumSystemSequenceStatusDailyTimeSheet.ActiveForOfficialUser.Id))
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckValidMonthTimesheetYearMonth(int yearmonth)
        {
            return _kscHrContext.WorkCalendars.Any(a => a.YearMonthV1 == yearmonth && a.SystemSequenceStatusId.Value == EnumSystemSequenceStatusDailyTimeSheet.TimeSheetIsCreated.Id);
        }

        public IQueryable<WorkCalendar> IsHoliday(DateTime date)
        {
            var day = _kscHrContext.WorkCalendars.Where(x => x.MiladiDateV1 == date).Include(x => x.WorkDayType).AsQueryable();
            return day;
        }

        public int AddMonthShamsi(int yearMonth,int monthToAdd)
        {
            var yearmonthShamsi = yearMonth.ToString();
            var year = int.Parse(yearmonthShamsi.Substring(0, 4));
            var month = int.Parse(yearmonthShamsi.Substring(4, 2));

            month += monthToAdd;
            if (month > 12)
            {
                month -= 12;
                year++;
            }
            if (month < 1)
            {
                month += 12;
                year--;
            }

            string newMonth=month.ToString().PadLeft(2, '0');
            return int.Parse($"{year}{newMonth}");

        }

        public int CalculateMonthDifference(string date1, string date2)
        {
            int date1year = int.Parse(date1.Substring(0, 4));
            int date1month = int.Parse(date1.Substring(4, 2));

            int date2year = int.Parse(date2.Substring(0, 4));
            int date2month = int.Parse(date2.Substring(4, 2));
            

            int yearDiff = date2year - date1year;
            int monthDiff = date2month - date1month;

            // محاسبه کل ماه‌های اختلاف
            return yearDiff * 12 + monthDiff;

        }
    }
}
