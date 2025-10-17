




using System;

namespace Ksc.HR.DTO.Personal.EmployeeEfficiencyHistory
{
    public class EmployeeEfficiencyHistoryModel
    {
        public int Id { get; set; } // Id (Primary key)
        public int EmployeeId { get; set; } // EmployeeId
        public int YearMonth { get; set; } // YearMonth
        public decimal Efficiency { get; set; } // Efficiency
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertDateShamsi { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)

        public bool IsLatest { get; set; } // IsLatest

    }
}
