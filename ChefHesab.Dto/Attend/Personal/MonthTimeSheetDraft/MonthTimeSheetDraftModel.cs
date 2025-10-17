using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.MonthTimeSheetDraft
{
    public class MonthTimeSheetDraftModel
    {
        public int WorkTimeId { get; set; }
        public int EmployeeId { get; set; }
        public int WorkCalendarId { get; set; }
        public int ForcedOverTime { get; set; }
        public int TotalWorkHourInWeek { get; set; }
        public int YearMonth { get; set; }
        public bool InsertData { get; set; }
        public bool ShiftSettingFromShiftboard { get; set; }
        public List<EmployeeAttendAbsenceItemForcedOverTimeModel> EmployeeAttendAbsenceItemForcedOverTimeModel { get; set; }
        public List<int> RollCallIncludedForcedOverTime { get; set; }
        public string MaximumForcedOverTime {get; set; }
        public bool FromDailyItem { get; set; }
    }
}
