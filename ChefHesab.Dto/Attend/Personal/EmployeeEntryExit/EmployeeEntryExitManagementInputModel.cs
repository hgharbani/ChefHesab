using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{
    public class EmployeeEntryExitManagementInputModel
    {
        public int EmployeeId { get; set; }
        public DateTime EntryExitDate { get; set; }
        public string EntryExitDateString { get; set; }
        public string TeamWorkCode { get; set; }
        public string CurrentUserName { get; set; }




        public int WorkCalendarId { get; set; }
        public int ShiftConceptDetailId { get; set; }
        public int WorkGroupId { get; set; }
        public int WorkCityId { get; set; }
        public bool IsValidEditEntryExit { get; set; }
        public List<int> RollCallDefinitionCeilingOvertime { get; set; }

    }
}
