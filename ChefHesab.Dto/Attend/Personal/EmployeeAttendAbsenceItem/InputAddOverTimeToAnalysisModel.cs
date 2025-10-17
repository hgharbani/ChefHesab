using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class InputAddOverTimeToAnalysisModel
    {
        public List<EmployeeAttendAbsenceAnalysisModel> resultModel { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int shiftConceptDetailId { get; set; }
        public RollCallModelForEmployeeAttendAbsenceModel overTime { get; set; }
        public long entryId { get; set; }
        public long exitId { get; set; }
        public TimeSettingDataModel timeSettingDataModel { get; set; }
        public RollCallForEmployeeAttendAbsenceModel allRollCall { get; set; }
        public EmployeeAttendAbsenceAnalysisInputModel analysisInputModel { get; set; }
        public bool TemprorayOverTimeInStartShift { get; set; }
        public bool TemprorayOverTimeInEndShift { get; set; }
    }
}
