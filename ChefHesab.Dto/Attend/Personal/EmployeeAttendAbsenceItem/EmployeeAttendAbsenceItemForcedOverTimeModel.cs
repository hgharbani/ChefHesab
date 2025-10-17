using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class EmployeeAttendAbsenceItemForcedOverTimeModel
    {
        public long EmployeeAttendAbsenceItemId { get; set; }
        public int RollCallDefinitionId { get; set; }
        public int TimeDurationInMinute { get; set; }
        public int WorkTimeId { get; set; }
        public int WorkCalendarId { get; set; }
        public DateTime WorkCalendarDate { get; set; }
        public int EmployeeId { get; set; }
    }
}
