using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{
    public class SearchEmployeeEntryExitModel : FilterRequest
    {
        public SearchEmployeeEntryExitModel()
        {
            EmployeeIds = new List<int>();
            RollCallDefinitionIds = new List<int>();
        }
        
        public string PersonalNumber { get; set; } // PersonalNumber (length: 10)
        public string WorkCalendarId { get; set; }

        public int EmployeeId { get; set; }
        public List<int> EmployeeIds { get; set; }
        public string TeamWorkCode { get; set; }
        public string ToTeamWorkCode { get; set; }
        public int? TeamWorkId { get; set; }
        public DateTime? EntryExitDate { get; set; }
        public string EntryExitDateString { get; set; }
        public string CurrentUser { get; set; }
        public bool IsOfficialAttendAbcense { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string EndDateString { get; set; }
        public string StartDateString { get; set; }
        //public List<string> RoleNames { get; set; }
        public string Name { get; set; }

        public string Family { get; set; }

        public string DisplayMember { get { return $"{PersonalNumber} {this.Name/*.Trim()*/} {this.Family/*.Trim()*/} "; } }
        public string YearMonth { get; set; }
        public bool IsOfficialAttend { get; set; }

        public int IsCreatedManualNum { get; set; }
        public List<int> RollCallDefinitionIds { get; set; }

    }
}
