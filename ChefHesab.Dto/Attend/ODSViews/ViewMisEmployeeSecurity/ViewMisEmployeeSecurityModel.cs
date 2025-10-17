using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.ODSViews
{
    // View_MIS_EmployeeSecurity
    public class ViewMisEmployeeSecurityModel 
    {
        public string Id { get; set; } // Id (length: 100)
        public int EmployeeNumber { get; set; } // EmployeeNumber
        public string TeamCode { get; set; } // TeamCode
        public string TeamTitle { get; set; } // TeamTitle (length: 60)
        public decimal? AuthenticationSecurity { get; set; } // AuthenticationSecurity
        public decimal? DisplaySecurity { get; set; } // DisplaySecurity
        public string WindowsUser { get; set; } // WindowsUser (length: 20)
        public int TeamWorkId { get; set; } // TeamWorkId
        public bool TeamWorkIsActive { get; set; } // TeamWorkIsActive
        public string MisUser { get; set; } // MisUser (length: 20)

    }

}
 
