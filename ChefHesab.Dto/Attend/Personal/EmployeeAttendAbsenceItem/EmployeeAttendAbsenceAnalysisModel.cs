using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using Ksc.HR.DTO.WorkShift.ShiftConceptDetail;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class EmployeeAttendAbsenceAnalysisModel : FilterRequest
    {
        public long EmployeeAttendAbsenceItemId { get; set; }
        public int EmployeeId { get; set; }
        public int WokCalendarId { get; set; }
        public string SelectedDateString { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }
        public int ShiftConceptDetailId { get; set; }
        //public string ShiftCodeDetails { get; set; }
        //  public SearchShiftConceptDetailModel ShiftCodeDetailsmodel { get; set; }
        [DisplayName("کد حضور و غیاب")]
        public RollCallDefinicationInItemModel RollCallDefinicationInItemModel { get; set; }
        public int RollCallDefinitionId { get; set; }
        //public string RollCallDefinitionCode { get; set; }
        //public string RollCallDefinitionTitle { get; set; }
        public bool DeleteIsValid { get; set; }
        public bool ModifyIsValid { get; set; }
        public int RollCallConceptId { get; set; }
        public long EntryId { get; set; }
        public long ExitId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsOfficialAttend { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsShiftStart { get; set; }
        public bool IsShiftEnd { get; set; }
        public string OverTimeToken { get; set; }
        public int ForcedOverTime { get; set; }
        public int TotalWorkHourInWeek { get; set; }
        public int YearMonth { get; set; }
        public bool ShiftSettingFromShiftboard { get; set; }
        public bool IsEntryExit { get; set; }
        public string CurentUserName { get; set; }
        public List<string> RolesForUser { get; set; }
        public bool InvalidForcedOvertime { get; set; }
        public double? AttendTimeInTemprorayTime { get; set; }
        public bool TemprorayOverTimeInStartShift { get; set; }
        public bool TemprorayOverTimeInEndShift { get; set; }

    }
}
