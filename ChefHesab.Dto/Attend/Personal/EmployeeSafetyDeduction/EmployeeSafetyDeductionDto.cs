using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.EmployeeSafetyDeduction
{
    public class EmployeeSafetyDeductionDto
    {
        public int Id { get; set; } // Id (Primary key)
        public int? EmployeeId { get; set; } // EmployeeId
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)
        public int YearMonth { get; set; }
        public int SumNegativeScore { get; set; }
        public int SumPercent { get; set; }
        public int ActionId { get; set; }
        public DateTime? CreateDateTime { get; set; } // CreateDateTime
        public string CreateUser { get; set; } // CreateUser (length: 50)
    }
}
