using Ksc.HR.Share.Model.TimeShiftSetting;
using Ksc.HR.Share.Model.WorkCalendar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class TimeShiftDateTimeSettingModel
    {
        public int employeeId { get; set; }
        public string employeeNumber { get; set; }
        public int shiftConceptDetailId { get; set; }
        public int workGroupId { get; set; }
        public DateTime date { get; set; }
        public List<TimeShiftSettingByWorkCityIdModel> timeShiftSettingByWorkCityIdModel { get; set; }
        public List<WorkCalendarForAttendAbcenseAnalysis> workCalendars { get; set; }
        public int? FloatTimeSettingId { get; set; }
        //public bool IsBreastfeddingInStartShift { get; set; }
        //public bool BreastfeddingOption { get; set; }
        public bool ValidConditionalAbsenceInStartShift { get; set; }
        public bool ValidConditionalAbsence { get; set; }
        public bool ValidConditionalAbsenceForBreastfedding { get; set; }
        public string ConditionalAbsenceToleranceTime { get; set; }
        public int WorkCalendarId { get; set; }
        public bool IsOfficialAttendForOverTime { get; set; }
        public bool IsValidHolidayValidOverTime { get; set; }

    }
}
