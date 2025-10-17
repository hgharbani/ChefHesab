using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkShift.Province;

namespace Ksc.HR.DTO.WorkShift.WorkDayTimeSetting
{
    public class WorkDayTimeSettingModel
    {
        public int Id { get; set; }
        public int WorkTimeId { get; set; }
        public int WorkDayTypeId { get; set; }
        public double? ExtraWorkPercentage { get; set; }
        /// <summary>
        /// درصد اضافه کار در شب
        /// </summary>
        public double? ExtraWorkPercentageInNight { get; set; }
        /// <summary>
        /// درصد اضافه کار در روز
        /// </summary>
        public double? ExtraWorkPercentageInDay { get; set; }
        public DateTime? InsertDate { get; set; }
        public string? InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public string? DomainName { get; set; }
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; } = null!;

        //public virtual WorkDayType WorkDayType { get; set; } = null!;
        //public virtual WorkTime WorkTime { get; set; } = null!;
    }
}
