using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Personal
{
    public class MonthTimeSheetLog : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// تاریخ زمانبندی
        /// </summary>
        public int? YearMonth { get; set; } // YearMonth
        public int MonthTimeShitStepperId { get; set; } // MonthTimeShitStepperId
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public string Result { get; set; } // Result
        public int ResultCount { get; set; } // ResultCount

        // Foreign keys

        /// <summary>
        /// Parent MonthTimeShitStepper pointed by [MonthTimeSheetLog].([MonthTimeShitStepperId]) (FK_MonthTimeSheetLog_MonthTimeShitStepper)
        /// </summary>
        public virtual MonthTimeShitStepper MonthTimeShitStepper { get; set; } // FK_MonthTimeSheetLog_MonthTimeShitStepper
    }
}
