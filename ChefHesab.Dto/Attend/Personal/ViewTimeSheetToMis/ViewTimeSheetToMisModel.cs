using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.ViewTimeSheetToMis
{
    public class ViewTimeSheetToMisModel
    {
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)
        public int? IncludedDefinitionId { get; set; } // IncludedDefinitionId
        public int? DurationInHour { get; set; } // DurationInHour
        public int? DurationInDay { get; set; } // DurationInDay
        public string DurationInHourFormatted { get; set; } // DurationInHourFormatted (length: 10)
        public int? WorkTimeId { get; set; } // WorkTimeId
        public int? DayCount { get; set; } // DayCount
        public int? ShiftBenefitInMinute { get; set; } // ShiftBenefitInMinute
        public int? RollCallDefinitionId { get; set; } // RollCallDefinitionId
        public int? DurationInMinut { get; set; } // DurationInMinut
        public int? DayCountInDailyTimeSheet { get; set; } // DayCountInDailyTimeSheet
        public string Duration { get; set; } // Duration (length: 10)
        public decimal SalaryAccountCode { get; set; } // SalaryAccountCode
        public int EmployeeId { get; set; } // EmployeeId
        public int YearMonth { get; set; } // YearMonth
        public int PaymentStatusId { get; set; } // PaymentStatusId
        public int WorkGroupId { get; set; } // WorkGroupId
        public string ExcessOverTime { get; set; } // ExcessOverTime (length: 6)
        public string SumInvalidOverTimInDailyTimeSheet { get; set; } // SumInvalidOverTimInDailyTimeSheet (length: 6)
        public string AverageBalanceOverTime { get; set; } // AverageBalanceOverTime (length: 6)
        public string ExcessOverTimeOld { get; set; } // ExcessOverTimeOld (length: 6)
        public int? EmploymentTypeId { get; set; } // EmploymentTypeId
    }
}
