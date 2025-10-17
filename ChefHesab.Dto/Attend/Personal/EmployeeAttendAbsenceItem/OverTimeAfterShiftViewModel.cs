using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class OverTimeAfterShiftViewModel
    {
        public List<EmployeeAttendAbsenceAnalysisModel> resultModel { get; set; }
        public TimeSettingDataModel timeSettingDataModel { get; set; }
        public EmployeeEntryExitViewModel entryExitResult { get; set; }
        public int ShiftConceptDetailId { get; set; }
        public RollCallModelForEmployeeAttendAbsenceModel karKardCode { get; set; }
        public RollCallModelForEmployeeAttendAbsenceModel defaultOverTime { get; set; }
        public RollCallForEmployeeAttendAbsenceModel allRollCall { get; set; }
        public int? employmentTypeId { get; set; }
        public RollCallModelForEmployeeAttendAbsenceModel karKardCodeTomorrow { get; set; }
        public List<RollCallModelForEmployeeAttendAbsenceModel> karKardToOverTimeToday { get; set; }
        public List<RollCallModelForEmployeeAttendAbsenceModel> karKardToOverTimeTomorrow { get; set; }
        public EmployeeAttendAbsenceAnalysisInputModel analysisInputModel { get; set; }
    }
}
