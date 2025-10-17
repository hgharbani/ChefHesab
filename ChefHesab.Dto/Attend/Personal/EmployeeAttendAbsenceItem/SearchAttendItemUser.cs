using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class SearchAttendItemUser
    {       
        public int EmployeeId { get; set; }
        public DateTime WorkCalendarDate { get; set; }
        public string TeamWorkCode { get; set; }
    }
}
