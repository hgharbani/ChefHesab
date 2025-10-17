using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public  class EmployeeAttendAbsenceItemForOnCallViewModel
    {
        public int EmployeeId { get; set; }
        public int OnCallTypeId { get; set; }
        public string OnCallDuration { get; set; }
        public string DayType { get; set; }
        public string StartTime { get; set; }
        public DateTime StartDate { get; set; }
        public int RollCallDefinitionIdForOnCall { get; set; }
        public long EmployeeAttendAbsenceItemId { get; set; }
    }
}
