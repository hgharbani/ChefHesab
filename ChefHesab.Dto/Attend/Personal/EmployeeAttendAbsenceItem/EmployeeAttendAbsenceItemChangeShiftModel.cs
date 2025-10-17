using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class EmployeeAttendAbsenceItemChangeShiftModel
    {
        public int ShiftConceptDetailId { get; set; } // ShiftConceptDetailId
        public string StartTime { get; set; } // StartTime (length: 5)
        public string EndTime { get; set; } // EndTime (length: 5)
        public int WorkCalendarId { get; set; } // WorkCalendarId
        public string TotalWorkHourInDay { get; set; } 

    }
}
