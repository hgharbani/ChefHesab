using Ksc.HR.DTO.WorkShift.WorkDayType;
using Ksc.HR.DTO.WorkShift.WorkTime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ksc.HR.DTO.WorkShift.InvalidDayTypeInForcedOvertime
{
    public class AddInvalidDayTypeInForcedOvertimeModel
    {
        public int Id { get; set; }
        [Description("نوع روزکاری")]
        public int WorkDayTypeId { get; set; }
        [Description("زمان کاری")]
        public int WorkTimeId { get; set; }
        public bool IsActive { get; set; }
        public string CurrentUserName { get; set; }
        public string DomainName { get; set; }
        public List<SearchWorkTimeModel> AvilibaleSearchWorkTimeMode { get; set; }
        public List<SearchWorkDayTypeModel> AvilibaleSearchWorkDayTypeMode { get; set; }
    }
}
