using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class TimeSheetViewModel
    {
        public string TimesheetDate { get; set; }
        public string TimesheetDay { get; set; }
        public List<string> TimeEntry { get; set; }
        public List<string> TimeExit { get; set; }
        public string TimesheetDuration { get; set; }
        public string VacationPerHour { get; set; }
        public string OfficeExit { get; set; }
        public string OverTime { get; set; }
        public string AbsencePerHour { get; set; }
        public string TimeSheetApprovalDescription { get; set; }
        public decimal? TimeSheetApprovalCode { get; set; }
        public List<int> CouponTypeId { get; set; }
        public List<string> TimeEntryExit { get; set; }
        public List<string> TypeEntryExit { get; set; }

    }
}
