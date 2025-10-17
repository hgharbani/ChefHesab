using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.ODSViews.ViewMisEmployee
{
    public class ViewMisEmployeeForFinancialModel 
    {
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)
        public string FirstName { get; set; } // FirstName (length: 20)
        public string LastName { get; set; } // LastName (length: 25)
        public string NationalCode { get; set; } 
        public string CostCenterCode { get; set; } 
        public string AccountNumber { get; set; } 
        public decimal? PaymentStatusCode { get; set; } // PaymentStatusCode
    }
}
