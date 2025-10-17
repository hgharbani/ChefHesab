
using KSC.Common.Filters.Models;
using System;
using System.ComponentModel;


namespace Ksc.HR.DTO.Personal.EmployeeEfficiencyHistory
{
    public class SearchEmployeeEfficiencyHistoryModel : FilterRequest
    {
        public int Id { get; set; } // Id (Primary key)
        public int EmployeeId { get; set; } // EmployeeId
        public int YearMonth { get; set; } // YearMonth
        public decimal Efficiency { get; set; } // Efficiency
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public string CurrentUserName { get; set; }

        public bool IsLatest { get; set; } // IsLatest

    }

}
