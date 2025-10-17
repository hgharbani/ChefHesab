using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class TemprorayOverTimeModel
    {
     public   List<EmployeeAttendAbsenceAnalysisModel> resultModel { get; set; }

        public TimeSettingDataModel timeSettingDataModel { get; set; }
        public RollCallModelForEmployeeAttendAbsenceModel rollCallTemporaryStartDate { get; set; }
        public RollCallModelForEmployeeAttendAbsenceModel rollCallTemporaryEndDate { get; set; }
        public EmployeeAttendAbsenceAnalysisInputModel inputModel { get; set; }
        public RollCallForEmployeeAttendAbsenceModel allRollCall { get; set; }
        public bool IsTemporaryOverTime { get; set; }
        public List<EmployeeEntryExitViewModel> entryExitResult { get; set; }
    }
}
