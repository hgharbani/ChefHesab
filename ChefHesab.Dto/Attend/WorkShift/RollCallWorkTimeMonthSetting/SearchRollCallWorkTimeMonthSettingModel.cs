using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.WorkShift.RollCallWorkTimeMonthSetting
{
    public class SearchRollCallWorkTimeMonthSettingModel : FilterRequest
    {
        public int Id { get; set; } 
        public int RollCallDefinitionId { get; set; }  
        public string RollCallDefinitionTitle { get; set; }  
        public string WorkTimeTitle { get; set; }  
        public int DurationInMinute { get; set; }  
        public string Duration { get; set; } 
        public bool IsActive { get; set; } // IsActive
    }
}
