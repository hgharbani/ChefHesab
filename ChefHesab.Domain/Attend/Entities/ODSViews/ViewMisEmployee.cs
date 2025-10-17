using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.ODSViews
{
    public class ViewMisEmployee : IEntityBase
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
        public string TeamCode { get; set; } // WinUser (length: 4)
        public string TeamTitle { get; set; } // WinUser (length: 4)


        public decimal? HrMonthTimeSheet { get; set; } // HrMonthTimeSheet 
        public int EmployeeId { get; set; }
        public string JobStatusDescription { get; set; } // JobStatusDescription (length: 13)
        public int? CategoryId { get; set; } // CategoryId
        public string CategoryTitle { get; set; } // CategoryTitle (length: 5)

        public decimal? JobLevelCode { get; set; } // JobLevelCode



        public string CostCenterCode { get; set; }
        public string AccountNumber { get; set; }
        public string NationalCode { get; set; }

    }
}
