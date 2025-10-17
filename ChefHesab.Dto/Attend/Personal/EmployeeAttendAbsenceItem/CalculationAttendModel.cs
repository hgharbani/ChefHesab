using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class TemporaryShiftSettings
    {
        public string ShiftStartTime { get; set; }
        public string ShiftEndTime { get; set; }
        public int? RollCallDefinitionStartShift { get; set; }
        public int? RollCallDefinitionEndShift { get; set; }
        public string BreastfeddingToleranceTime { get; set; }
        public bool? CheckedEmployeeValidOverTime { get; set; }
    }

    public class TimeRanges
    {
        public string TimeBeforeShiftStart { get; set; }
        public string TimeAfterShiftEnd { get; set; }
        public string ShiftStartWithTolerance { get; set; }
        public string ShiftEndWithTolerance { get; set; }
    }

    public class SpecialConditions
    {
        public bool IsRestShift { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsUnOfficialHoliday { get; set; }
        public bool InvalidOverTime { get; set; }
    }

    public class ContinuousShiftResult
    {
        public bool IsContinuous { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTime SuggestedExitTime { get; set; }
    }

    public class ShiftContinuityCheckInput
    {
        public EmployeeEntryExitViewModel LastToday { get; set; }
        public EmployeeEntryExitViewModel FirstTomorrow { get; set; }
        public TimeSettingDataModel TimeSettings { get; set; }
        public EmployeeAttendAbsenceAnalysisInputModel Input { get; set; }
    }
}
