
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.WorkDayTimeSetting
{
    public class EditWorkDayTimeSettingModel
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
        public DateTime? UpdateDate { get; set; }
        public string CurrentUserName { get; set; }
        public string? DomainName { get; set; }
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; } = null!;

        //public virtual WorkDayType WorkDayType { get; set; } = null!;
        //public virtual WorkTime WorkTime { get; set; } = null!;

    }
}
