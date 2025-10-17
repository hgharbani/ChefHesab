using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.ODSViews.ViewMisEmployeeSecurity
{
    public class UserWindowTeamAccess : FilterRequest
    {
        public string Id { get; set; } // Id (length: 100)
        public string EmployeeNumber { get; set; } // EmployeeNumber
        public string NameFamily { get; set; }
        public string TeamCode { get; set; } // TeamCode
        public string TeamTitle { get; set; } // TeamTitle (length: 60)
        public int? JobPositionId { get; set; }
        public string StructureTitle { get; set; }
        public string StructureCode { get; set; }
        public decimal CostCenter { get; set; }
    }
}
