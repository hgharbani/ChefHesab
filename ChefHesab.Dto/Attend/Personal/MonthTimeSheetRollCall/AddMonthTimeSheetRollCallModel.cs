using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.MonthTimeSheetRollCall
{
    public class AddMonthTimeSheetRollCallModel
    {
        public string Id { get; set; } // Id (Primary key)

        /// <summary>
        /// شناسه تایم-شیت ماهیانه
        /// </summary>
        public int MonthTimeSheetId { get; set; } // MonthTimeSheetId
        /// <summary>
        /// کد حضور غیاب
        /// </summary>
        [DisplayName("کد حضور غیاب")]
        public int? RollCallDefinitionId { get; set; }
        /// <summary>
        /// مدت زمان
        /// </summary>
        [DisplayName("مدت زمان")]

        public int? DurationInMinut { get; set; } // DurationInMinut

        /// <summary>
        /// تعداد روز در کارکرد روزانه
        /// </summary>
        [DisplayName("تعداد روز")]

        public int? DayCountInDailyTimeSheet { get; set; } // DayCountInDailyTimeSheet

        /// <summary>
        /// مدت زمان
        /// </summary>
        [DisplayName("مدت زمان")]
        public string Duration { get;  set; } // Duration (length: 10)
    }
}
