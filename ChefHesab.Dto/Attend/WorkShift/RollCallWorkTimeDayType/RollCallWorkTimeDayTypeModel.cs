using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.RollCallWorkTimeDayType
{
    public class RollCallWorkTimeDayTypeModel
    {
        public int Id { get; set; }
        /// <summary>
        /// نوع روز کاری
        /// </summary>
        public int WorkDayTypeId { get; set; }
        /// <summary>
        /// زمان کاری
        /// </summary>
        public int WorkTimeId { get; set; }
        public string WorkDayTypeTitle { get; set; }
        public string WorkTimeTitle { get; set; }

    }
}
