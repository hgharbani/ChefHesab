using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.MonthTimeSheet
{
    public class EmployeeItemGroupModel
    {
        public int RollCallDefinitionId { get; set; }
        public long EmployeeId { get; set; }
        public double TotalDuration { get; set; }
        public double TotalDuration1 { get; set; }
         public int DayCountInDailyTimeSheet { get; set; }

        public int WorkTimeId { get; set; }
        public int WorkTimeDays { get; set; }
        public int ShiftBenefitInMinute { get; set; }


        public int IncludedDefinitionId { get; set; }
        public int? DurationInHour { get; set; }
        public int? DurationInDay { get; set; }
        public int IncreasedTimeDuration { get; set; }


    }
}
