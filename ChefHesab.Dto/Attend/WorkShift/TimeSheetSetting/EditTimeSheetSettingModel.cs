using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.TimeSheetSetting
{
    public class EditTimeSheetSettingModel
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// مرخصی استحقاقی در هر ماه
        /// </summary>
        [DisplayName("مرخصی استحقاقی در هر ماه")]
        public string VacationEntitlementTimePerMonth { get; set; } // VacationEntitlementTimePerMonth (length: 5)

        /// <summary>
        /// زمان فرجه شیردهی
        /// </summary>
        [DisplayName("زمان فرجه شیردهی")]
        public string BreastfeddingToleranceTime { get; set; } // BreastfeddingToleranceTime (length: 5)

        /// <summary>
        /// حداقل اضافه کار بعد از شیفت به دقیقه
        /// </summary>
        [DisplayName("حداقل اضافه کار بعد از شیفت به دقیقه")]
        public int MinimumOverTimeAfterShiftInMinut { get; set; } // MinimumOverTimeAfterShiftInMinut
        /// <summary>
        /// حداقل زمان قبل از شروع شیفت
        /// </summary>
        [DisplayName("حداقل زمان قبل از شروع شیفت")]
        public int MinimumShiftStartTimeInMinute { get; set; } // MinimumShiftStartTimeInMinute

        /// <summary>
        /// اضافه کار قهری شاخص
        /// </summary>
        [DisplayName("اضافه کار قهری شاخص")]
        public string ForcedOverTimeBasic { get; set; } // ForcedOverTimeBasic (length: 5)

        /// <summary>
        /// مدیت یک روز کاری
        /// </summary>
        [DisplayName("مدت یک روز کاری")]
        public string WorkDayDuration { get; set; } // WorkDayDuration (length: 5)

        /// <summary>
        /// حداقل مانده مرخصی روزانه
        /// </summary>
        [DisplayName("حداقل مانده مرخصی روزانه")]
        public int MinimumDailyVacation { get; set; } // MinimumDailyVacation
        //public DateTime? InsertDate { get; set; } // InsertDate
        //public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string DomainName { get; set; } // DomainName (length: 50)
        public bool IsActive { get; set; } // IsActive}
    }
}