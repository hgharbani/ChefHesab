using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeTimeSheet
{
    public class EmployeeTimeSheetMonthModel
    {
        public int? Id { get; set; }
        public long EmployeeId { get; set; } // EmployeeId
        public int YearMonth { get; set; } // YearMonth

        /// <summary>
        /// اضافه کار سقف
        /// </summary>
        public double CeilingOvertime { get; set; } // CeilingOvertime

        /// <summary>
        /// اضافه کار مازاد سقف
        /// </summary>
        public double ExcessOverTime { get; set; } // ExcessOverTime

        public int? AverageBalanceOverTime { get; set; }
        public string AverageBalanceOverTimeDuration { get; set; }

        public int? TrainingOverTime { get; set; }
        public string TrainingOverTimeDuration { get; set; }

        public string CurrentUser { get; set; }
        public double CeilingOvertime1 { get; set; } // CeilingOvertime

        /// <summary>
        /// اضافه کار مازاد سقف
        /// </summary>
        public double ExcessOverTime1 { get; set; } // ExcessOverTime
    }
}
