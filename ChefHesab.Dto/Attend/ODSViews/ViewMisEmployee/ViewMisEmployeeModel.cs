using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.ODSViews.ViewMisEmployee
{
    public class ViewMisEmployeeModel 
    {
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)
        //public string FirstName { get; set; } // FirstName (length: 20)
        //public string LastName { get; set; } // LastName (length: 25)
        public string JobPositionCode { get; set; } // JobcPositionCode (length: 13)
        public string JobPositionTitle { get; set; } // JobPositionTitle (length: 60)
        public int? CategoryId { get; set; } // CategoryId
        public string CategoryTitle { get; set; } // CategoryTitle (length: 5)
    }
}
