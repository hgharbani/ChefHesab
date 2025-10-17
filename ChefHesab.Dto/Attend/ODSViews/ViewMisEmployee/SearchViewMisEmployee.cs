using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.ODSViews.ViewMisEmployee
{
    public class SearchViewMisEmployee : FilterRequest
    {
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)
        public string FirstName { get; set; } // FirstName (length: 20)
        public string LastName { get; set; } // LastName (length: 25)
        public string JobPositionCode { get; set; } // JobcPositionCode (length: 13)
        public string JobPositionTitle { get; set; } // JobPositionTitle (length: 60)
        public string JobCategoryCode { get; set; } // JobCategoryCode (length: 2)
        public string JobCategoryTitle { get; set; } // JobCategoryTitle (length: 30)
        public string JobStatusCode { get; set; } // JobStatusCode (length: 50)
        public decimal? PaymentStatusCode { get; set; } // PaymentStatusCode
        public string MisUser { get; set; } // MisUser (length: 8)
        public string WinUser { get; set; } // WinUser (length: 20)
    }
}
