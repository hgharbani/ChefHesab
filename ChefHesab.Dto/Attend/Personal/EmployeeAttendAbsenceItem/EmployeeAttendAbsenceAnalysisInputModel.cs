using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class EmployeeAttendAbsenceAnalysisInputModel : FilterRequest
    {
        public int EmployeeId { get; set; }
        public string TeamWorkCode { get; set; }
        public string Date { get; set; }
        public string EntryExitDateString { get; set; }
        public int WorkCalendarId { get; set; }
        public int ShiftConceptDetailId { get; set; }
        public int WorkGroupId { get; set; }
        public int WorkCityId { get; set; }
        public string CurrentUserName { get; set; }
        public string Domain { get; set; }
        public List<string> Roles { get; set; }
        public int AnalysisType { get; set; }
        /// <summary>
        /// نقش اداری برای دسنرسی به تمام فانکشنهای حضور-غیاب 
        /// </summary>
        public bool IsOfficialAttend { get; set; }
        public bool IsOfficialAttendForOverTime { get; set; }
        public bool IsValidHolidayValidOverTime { get; set; }
        public bool NotCheckMinimumWorkTimeAdmin { get; set; }
        public string IsAbsenceRow { get; set; }
}
}
