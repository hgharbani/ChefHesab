using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.EmployeeSafetyDeduction
{
    public class SafetyPerformanceMonthlyVM
    {
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)
        public int YearMonth { get; set; }
        public int SumNegativeScore { get; set; }
        public int SumPercent { get; set; }
        public int ActionId { get; set; }
        public string CurrentUser { get; set; }
    }
}
