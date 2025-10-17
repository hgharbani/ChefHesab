using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class EmployeeDontHaveAttendITemModel
    {
        public int EmployeeId { get; set; }
        public string TeamWorkCode { get; set; }
        public string Date { get; set; }
        public int WorkCalendarId { get; set; }
        public DateTime WorkCalendarDate { get; set; }
        public string FullName { get; set; }

        public string PersonalNumber { get; set; }

        public string TeamName { get; set; }
        public string ManagerTeam { get; set; }
        public string WorkGroupCode { get; set; }
        public string TeamCode { get; set; }
        public int TeamWorkId { get; set; }
    }
}
