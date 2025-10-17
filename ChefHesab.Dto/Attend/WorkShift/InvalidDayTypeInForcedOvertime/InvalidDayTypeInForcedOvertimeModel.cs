using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.WorkShift.InvalidDayTypeInForcedOvertime
{
    public class InvalidDayTypeInForcedOvertimeModel
    {
        public int Id { get; set; }
        public int WorkDayTypeId { get; set; }
        public int WorkTimeId { get; set; }
        public string WorkDayTypeTitle { get; set; }
        public string WorkTimeTitle { get; set; }
        public bool IsActive { get; set; }
    }
}
