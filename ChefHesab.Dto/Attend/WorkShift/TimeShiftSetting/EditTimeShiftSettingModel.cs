
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.TimeShiftSetting
{
    public class EditTimeShiftSettingModel
    {
        /// <summary>
        /// تنظیمات زمان شیفت
        /// </summary>

        public int Id { get; set; }
        [DisplayName("تنظیمات")]
        public int WorkCompanySettingId { get; set; }
        /// <summary>
        /// تاریخ شروع اعتبار
        /// </summary>
        [DisplayName("تاریخ شروع اعتبار")]
        public DateTime? ValidityStartDate { get; set; }
        /// <summary>
        /// تاریخ پایان اعتبار
        /// </summary>
        [DisplayName("تاریخ پایان اعتبار")]
        public DateTime? ValidityEndDate { get; set; }
        /// <summary>
        /// ساعت شروع کار
        /// </summary>
        [DisplayName("ساعت شروع کار(HH:MM)")]
        public string ShiftStartTime { get; set; }
        /// <summary>
        /// ساعت پایان کار
        /// </summary>
        [DisplayName("ساعت پایان کار(HH:MM)")]
        public string ShiftEndtTime { get; set; }
        ///// <summary>
        ///// مدت زمان قبل از شروع شیفت
        ///// </summary>
        //[DisplayName("مدت زمان قبل از شروع شیفت")]
        //public string? DurationTimeBeforeShiftStartTime { get; set; }
        ///// <summary>
        ///// مدت زمان بعد از پایان شیفت
        ///// </summary>
        //[DisplayName("")]
        //public string? DurationTimeAfterShiftEndTime { get; set; }
        /// <summary>
        /// فرجه ساعت شروع
        /// </summary>
        [DisplayName("فرجه ساعت شروع (به دقیقه)")]
        public int? ToleranceShiftStartTime { get; set; }
        /// <summary>
        /// فرجه ساعت پایان
        /// </summary>
        [DisplayName("فرجه ساعت پایان (به دقیقه)")]
        public int? ToleranceShiftEndTime { get; set; }
        /// <summary>
        /// درصد نوبت کاری
        /// </summary>
        [DisplayName("درصد نوبت کاری")]
        public int? PercentageWorkTime { get; set; }
        [DisplayName("مجموع ساعت کاری در روز(HH:MM)")]
        public string TotalWorkHourInDay { get; set; }
        [DisplayName("مجموع ساعت کاری در هفته(HH:MM)")]
        public string TotalWorkHourInWeek { get; set; }
        [DisplayName("مجموع ساعت کاری در ماه(HH:MM)")]
        public string TotalWorkHourInMonth { get; set; }
        [DisplayName("مجموع ساعت کاری در سال(HH:MM)")]
        public string TotalWorkHourInYear { get; set; }
        [DisplayName("اضافه کار قهری(HH:MM)")]
        public string ForcedOverTime { get; set; }
        [DisplayName("حداقل ساعت کارکرد در روز(HH:MM)")]
        public string MinimumWorkHourInDay { get; set; }
        [DisplayName("تاریخ درج")]
        public DateTime? InsertDate { get; set; }
        [DisplayName("تاریخ ویرایش")]
        public DateTime? UpdateDate { get; set; }
        public string CurrentUserName { get; set; }
        [DisplayName("دامنه")]
        public string? DomainName { get; set; }
        [DisplayName("وضعیت")]
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; } = null!;
        public bool IsTemporaryTime { get; set; }
        [DisplayName("کد حضور-غیاب در ابتدای شیفت")]
        public int? TemporaryRollCallDefinitionStartShift { get; set; } // TemporaryRollCallDefinitionStartShift
        [DisplayName("کد حضور-غیاب در پایان شیفت")]
        public int? TemporaryRollCallDefinitionEndShift { get; set; } // TemporaryRollCallDefinitionEndShift
        public List<SearchRollCallDefinicationModel> AvilableyTemporaryRollCallDefinitionStartShift { get; set; }
        public List<SearchRollCallDefinicationModel> AvilableyTemporaryRollCallDefinitionEndShift { get; set; }
        public List<SearchRollCallDefinicationModel> AvilableyTemprorayOverTimeRollCallDefinitionStartShift { get; set; }
        public List<SearchRollCallDefinicationModel> AvilableyTemprorayOverTimeRollCallDefinitionEndShift { get; set; }
        [DisplayName("مدت زمان مرخصی شیردهی(HH:MM)")]
        public string BreastfeddingToleranceTime { get; set; } // BreastfeddingToleranceTime (length: 5)
        [DisplayName("مدت زمان اضافه کار در پایان شیفت(HH:MM)")]
        public string TemprorayOverTimeDuration { get; set; }
        [DisplayName("چک کردن افراد در لیست مجوز اضافه کار")]
        public bool? CheckedEmployeeValidOverTime { get; set; } // CheckedEmployeeValidOverTime
        [DisplayName("کد اضافه کار در ابتدای شیفت")]
        public int? TemprorayOverTimeRollCallDefinitionStartShift { get; set; }
        [DisplayName("کد اضافه کار در پایان شیفت")]
        public int? TemprorayOverTimeRollCallDefinitionEndShift { get; set; }
        [DisplayName("زمان فرجه عدم حضور پرسنل با شرایط خاص(HH:MM)")]
        public string ConditionalAbsenceToleranceTime { get; set; }
        [DisplayName("ساعت شروع اضافه کار مجاز(HH:MM)")]
        public string ValidOverTimeStartTime { get; set; }
        [DisplayName("مدت زمان اضافه کار در شروع شیفت(HH:MM)")]
        public string TemprorayOverTimeDurationInStartShift { get; set; }
    }
}
