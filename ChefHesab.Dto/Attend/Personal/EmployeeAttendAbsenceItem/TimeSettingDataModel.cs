using Ksc.HR.Share.Model.DayNightRollCall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class TimeSettingDataModel
    {
        public string ShiftStartTime { get; set; }
        public string ShiftEndTime { get; set; }
        public TimeSpan ShiftStartTimeToTimeSpan { get; set; }
        public TimeSpan ShiftEndTimeToTimeSpan { get; set; }
        public DateTime ShiftStartDate { get; set; }
        public DateTime ShiftEndDate { get; set; }
        public int DayNumber { get; set; }
        public bool IsTemporaryTime { get; set; }
        public string TemporaryShiftStartTime { get; set; }
        public string TemporaryShiftEndTime { get; set; }
        public bool IsTemporaryOverTime { get; set; }
        public bool InvalidOverTime { get; set; }
        public string TemporaryShiftStartOverTimeInEndTime { get; set; }
        public string TemporaryShiftEndtTimeWithTolerance { get; set; }
        public string TemporaryShiftEndOverTimeInEndTime { get; set; }
        public int? TemporaryRollCallDefinitionStartShift { get; set; }
        public int? TemporaryRollCallDefinitionEndShift { get; set; }
        public string TimeBeforeShiftStartTime { get; set; }
        public DateTime DateBeforeShiftStartTime { get; set; }
        public string TimeAfterShiftEndTime { get; set; }
        public TimeSpan TimeAfterShiftEndTimeToTimeSpan { get; set; }
        public DateTime DateAfterShiftEndTime { get; set; }
        public string ShiftStartTimeWithTolerance { get; set; }
        public string ShiftEndTimeWithTolerance { get; set; }
        public TimeSpan ShiftStartTimeWithToleranceToTimeSpan { get; set; }
        public TimeSpan ShiftEndTimeWithToleranceToTimeSpan { get; set; }
        public bool IsRestShift { get; set; }
        public bool ShiftSettingFromShiftboard { get; set; }
        public bool IsOfficialHoliday { get; set; }
        public bool IsUnOfficialHoliday { get; set; }
        public bool IsHoliday { get; set; }
        public int WorkDayTypeId { get; set; }
        public int WorkTimeId { get; set; }
        //public string BreastfeddingStartTime { get; set; }
        //public string BreastfeddingEndTime { get; set; }
        //public TimeSpan BreastfeddingStartTimeToTimeSpan { get; set; }
        //public TimeSpan BreastfeddingEndTimeToTimeSpan { get; set; }
        //public DateTime BreastfeddingStartDate { get; set; }
        //public DateTime BreastfeddingEndDate { get; set; }

        public string TotalWorkHourInDay { get; set; }
        public TimeSpan TotalWorkHourInDayToTimeSpan { get; set; }
        public int? TemprorayOverTimeRollCallDefinitionStartShift { get; set; }
        public int? TemprorayOverTimeRollCallDefinitionEndShift { get; set; }
        public int DateKey { get; set; }

        /// <summary>
        /// حداقل ساعت شروع اضافه کاری
        /// </summary>
        public string MinimumOverTimeAfterShift { get; set; }
        /// <summary>
        /// حداقل ساعت شروع اضافه کاری
        /// </summary>
        public TimeSpan MinimumOverTimeAfterShiftToTimeSpan { get; set; }
        public string MinimumShiftStartTimeInMinute { get; set; }
        public TimeSpan MinimumShiftStartTimeInMinuteToTimeSpan { get; set; }
        public string WorkDayDuration { get; set; }
        public int MaximumAttendInMinute { get; set; }
        #region پارامترهای که با توجه به تاریخ ورودی (این فیلدها برای بررسی روز قبل و روز بعد استفاده میشوند) مقدار میگیرند
        public string TomorrowShiftStartTime { get; set; }
        public string TomorrowShiftEndTime { get; set; }
        public bool TomorrowIsRestShift { get; set; }
        public int TomorrowWorkDayTypeId { get; set; }
        public int TomorrowWorkTimeId { get; set; }
        public int TomorrowShiftConceptId { get; set; }
        public bool TomorrowExistEmployeeAttendAbsenceItem { get; set; }
        public DateTime TomorrowDateTime { get; set; }
        public TimeSpan TomorrowShiftStartTimeToTimeSpan { get; set; }
        public bool TomorrowIsOfficialHoliday { get; set; }
        public bool TomorrowIsUnOfficialHoliday { get; set; }
        public int TomorrowDayNumber { get; set; }
        public TimeSpan TomorrowShiftEndTimeToTimeSpan { get; set; }
        #endregion
        public int WorkGroupId { get; set; }
        public int ShiftCondeptId { get; set; }
        public int WorkCalendarIdToday { get; set; }
        public int WorkCompanySettingId { get; set; }
        public int WorkCityId { get; set; }

        /// <summary>
        /// معتبر بودن اضافه کاری در روزهای تعطیل غیر رسمی
        /// </summary>
        public bool IsValidOverTimeInUnOfficialHoliday { get; set; }
        /// <summary>
        /// تعطیلات رسمی-غیررسمی از تقویم کاری باشد
        /// </summary>
        public bool OfficialUnOfficialHolidayFromWorkCalendar { get; set; }
        public List<DayNightRollCallSettingModel> DayNightRollCallSettingModel { get; set; }
        public int ForcedOverTime { get; set; }
        public int TotalWorkHourInWeek { get; set; }
        public int YearMonth { get; set; }
        public int MaximumForcedOverTime { get; set; }
        public bool ShiftConceptIsRest { get; set; }
        /// <summary>
        /// زمان شناور نسبت به شروع شیفت
        /// </summary>
        public string FloatTimeFromShiftStart { get; set; }
        /// <summary>
        ///  حداکثر زمان شناور نسبت به شروع شیفت
        /// </summary>
        public string MaximumFloatTimeFromShiftStart { get; set; }
        /// <summary>
        /// زمان شناور نسبت به پایان شیفت
        /// </summary>
        public string FloatTimeFromShiftEnd { get; set; }
        /// <summary>
        /// ساعت شروع آموزش برای کارکرد
        /// </summary>
        public TimeSpan TrainingStartTimeToTimeSpan { get; set; }
        /// <summary>
        /// ساعت پایان آموزش برای کارکرد
        /// </summary>
        public TimeSpan TrainingEndTimeToTimeSpan { get; set; }
        public string TemporaryShiftEndtTimeReal { get; set; }
        public TimeSpan TemporaryShiftEndtTimeRealToTimeSpan { get; set; }
        public TimeSpan TemporaryShiftEndtTimeWithToleranceToTimeSpan { get; set; }
        public bool CheckedEmployeeValidOverTime { get; set; }
        public string TemporaryShiftStartTimeReal { get; set; }
        public TimeSpan TemporaryShiftStartTimeRealToTimeSpan { get; set; }


        public string ConditionalAbsenceStartTime { get; set; }
        public string ConditionalAbsenceEndTime { get; set; }
        public TimeSpan ConditionalAbsenceStartTimeToTimeSpan { get; set; }
        public TimeSpan ConditionalAbsenceEndTimeToTimeSpan { get; set; }
        public DateTime ConditionalAbsenceStartDate { get; set; }
        public DateTime ConditionalAbsenceEndDate { get; set; }
        public int MinimumOverTimeBeforeShiftInMinut { get; set; }
        public TimeSpan MinimumOverTimeBeforeShiftInMinutToTimeSpan { get; set; }
        public bool ValidConditionalAbsence { get; set; }
        public string ValidOverTimeStartTime { get; set; }
        public bool ValidOverTimeByEmployeeId { get; set; }
        public string MinimumWorkHourInDay { get; set; } // MinimumWorkHourInDay (length: 5)
        public string TemporaryShiftStartTimeWithTolerance { get; set; }
        public string TemporaryShiftStartOverTimeInStartTime { get; set; }
        public string TemporaryShiftEndOverTimeInStartTime { get; set; }
    }
}
