
using Ksc.HR.Resources.Workshift;
using KSC.Common.Filters.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ksc.HR.DTO.WorkShift.TimeShiftSetting
{
    public class TimeShiftSettingModel : FilterRequest
    {
        /// <summary>
        /// تنظیمات زمان شیفت
        /// </summary>

        public int Id { get; set; }

        [Display(Name = nameof(WorkCompanySettingId), ResourceType = typeof(TimeShiftSettingResource))]
        public int WorkCompanySettingId { get; set; }

        /// <summary>
        /// تاریخ شروع اعتبار
        /// </summary>
        [Display(Name = nameof(ValidityStartDate), ResourceType = typeof(TimeShiftSettingResource))]
        public DateTime? ValidityStartDate { get; set; }

        /// <summary>
        /// تاریخ پایان اعتبار
        /// </summary>
        [Display(Name = nameof(ValidityEndDate), ResourceType = typeof(TimeShiftSettingResource))]
        public DateTime? ValidityEndDate { get; set; }

        /// <summary>
        /// ساعت شروع کار
        /// </summary>
        [Display(Name = nameof(ShiftStartTime), ResourceType = typeof(TimeShiftSettingResource))]
        public string? ShiftStartTime { get; set; }

        /// <summary>
        /// ساعت پایان کار
        /// </summary>
        public string? ShiftEndtTime { get; set; }

        /// <summary>
        /// مدت زمان قبل از شروع شیفت
        /// </summary>
        [Display(Name = nameof(DurationTimeBeforeShiftStartTime), ResourceType = typeof(TimeShiftSettingResource))]
        public string? DurationTimeBeforeShiftStartTime { get; set; }
        [Display(Name = nameof(ForcedOverTime), ResourceType = typeof(TimeShiftSettingResource))]
        public string ForcedOverTime { get; set; }
        [Display(Name = nameof(MinimumWorkHourInDay), ResourceType = typeof(TimeShiftSettingResource))]
        public string MinimumWorkHourInDay { get; set; }
        /// <summary>
        /// مدت زمان بعد از پایان شیفت
        /// </summary>
        [Display(Name = nameof(DurationTimeAfterShiftEndTime), ResourceType = typeof(TimeShiftSettingResource))]
        public string? DurationTimeAfterShiftEndTime { get; set; }

        /// <summary>
        /// فرجه ساعت شروع
        /// </summary>
        [Display(Name = nameof(ToleranceShiftStartTime), ResourceType = typeof(TimeShiftSettingResource))]
        public int? ToleranceShiftStartTime { get; set; }

        /// <summary>
        /// فرجه ساعت پایان
        /// </summary>
        [Display(Name = nameof(ToleranceShiftEndTime), ResourceType = typeof(TimeShiftSettingResource))]
        public int? ToleranceShiftEndTime { get; set; }

        /// <summary>
        /// درصد نوبت کاری
        /// </summary>
        [Display(Name = nameof(PercentageWorkTime), ResourceType = typeof(TimeShiftSettingResource))]
        public int? PercentageWorkTime { get; set; }

        [Display(Name = nameof(TotalWorkHourInDay), ResourceType = typeof(TimeShiftSettingResource))]
        public string TotalWorkHourInDay { get; set; }

        [Display(Name = nameof(TotalWorkHourInWeek), ResourceType = typeof(TimeShiftSettingResource))]
        public string TotalWorkHourInWeek { get; set; }

        [Display(Name = nameof(TotalWorkHourInMonth), ResourceType = typeof(TimeShiftSettingResource))]
        public string TotalWorkHourInMonth { get; set; }

        [Display(Name = nameof(TotalWorkHourInYear), ResourceType = typeof(TimeShiftSettingResource))]
        public string TotalWorkHourInYear { get; set; }

        [Display(Name = nameof(InsertDate), ResourceType = typeof(TimeShiftSettingResource))]
        public DateTime? InsertDate { get; set; }

        [Display(Name = nameof(InsertUser), ResourceType = typeof(TimeShiftSettingResource))]
        public string? InsertUser { get; set; }


        [Display(Name = nameof(UpdateDate), ResourceType = typeof(TimeShiftSettingResource))]
        public DateTime? UpdateDate { get; set; }

        [Display(Name = nameof(UpdateUser), ResourceType = typeof(TimeShiftSettingResource))]
        public string? UpdateUser { get; set; }

        [Display(Name = nameof(PercentageWorkTime), ResourceType = typeof(TimeShiftSettingResource))]
        public string? DomainName { get; set; }

        [Display(Name = nameof(IsActive), ResourceType = typeof(TimeShiftSettingResource))]
        public bool IsActive { get; set; }

        public bool IsTemporaryTime { get; set; }

        public byte[] RowVersion { get; set; } = null!;

        //public virtual WorkCompanySetting WorkCompanySetting { get; set; } = null!;
    }
}
