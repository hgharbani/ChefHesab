using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.TimeShiftSetting
{
    public class ForcedOverTimeModel
    {
        public int ShiftConceptId { get; set; }
        public int WorkTimeId { get; set; }
        public int WorkCityId { get; set; }
        public string ForcedOverTime { get; set; }
        public string TotalWorkHourInWeek { get; set; }
    }
}
