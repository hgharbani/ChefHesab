using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{
    public class SearchReportEmployeeEntryExitModel: FilterRequest
    {
        public string YearMonth { get; set; }
        public string StartTeamCode { get; set; }
        public string EndTeamCode { get; set; }
        public string PersonalNumber { get; set; } // PersonalNumber (length: 10)
       // public bool ShowAnyTeam { get; set; }
        public string CurrentUserName { get; set; }
        public bool IsOfficialAttend { get; set; }
        public bool NotShowEmployeeInMonthSheet { get; set; }
    }
}
