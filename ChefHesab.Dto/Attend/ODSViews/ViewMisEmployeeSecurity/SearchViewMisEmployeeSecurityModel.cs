using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.ODSViews.ViewMisEmployeeSecurity
{
    public class SearchViewMisEmployeeSecurityModel : FilterRequest
    {
        public SearchViewMisEmployeeSecurityModel()
        {
            RollName = new List<string>();
        }
        public string CurrentUserName { get; set; }
        //[DisplayName("عنوان")]
        public string Id { get; set; } // Id (length: 100)
        public string EmployeeNumber { get; set; } // EmployeeNumber
        public string NameFamily { get; set; }
        public decimal? TeamCode { get; set; } // TeamCode
        public string TeamTitle { get; set; } // TeamTitle (length: 60)
        public decimal? AuthenticationSecurity { get; set; } // AuthenticationSecurity
        public decimal? DisplaySecurity { get; set; } // DisplaySecurity
        public string RoleName { get; set; }
        public bool IsOfficialAttendAbcense { get; set; }
        public bool IsSalaryUser { get; set; }
        public string WindowsUser { get; set; }

        public List<string> RollName { get; set; }
        public List<int> ids { get; set; }=new List<int>();
        public bool TeamWorkIsActive { get; set; }
        public string FullName { get; set; }
    }
}
