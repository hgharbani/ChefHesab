using Ksc.HR.DTO.WorkShift.RollCallWorkTimeDayType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.RollCallDefinication
{
    public class RollCallModelForEmployeeAttendAbsenceModel
    {
        public int RollCallDefinitionId { get; set; }
        public DateTime ValidityStartDate { get; set; }
        public DateTime ValidityEndDate { get; set; }
        public string RollCallDefinitionTitle { get; set; }
        public string RollCallDefinitionCode { get; set; }
        public int? RollCallCategoryId { get; set; }
        public int RollCallConceptId { get; set; }
        public bool IsValidForAllWorkTimeDayType { get; set; }
        public bool IsValidForAllCategoryCode { get; set; }
        public bool IsValidForAllEmploymentType { get; set; }
        public bool IsValidForTemporaryStartDate { get; set; }
        public bool IsValidForTemporaryEndDate { get; set; }
        public bool IsValidInShiftStart { get; set; }
        public bool IsValidInShiftEnd { get; set; }
        public bool IsValidSingleDelete { get; set; }
        public int? TimesAllowedUsePerDay { get; set; }
        public int? TimesAllowedUsePerWeek { get; set; }
        public int? TimesAllowedUsePerMonth { get; set; }
        public int? TrainingTypeId { get; set; }
        public bool TrainingValidInShiftTime { get; set; }
        public bool TrainingValidOutShiftTime { get; set; }
        public bool VaccinationCheck { get; set; }
        public int? WorkTimeId { get; set; }
        public int? WorkDayTypeId { get; set; }
        public decimal? EmploymentTypeCode { get; set; }
    }
}
