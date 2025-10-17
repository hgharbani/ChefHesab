using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class AddKarkardToAnalysisViewModel
    {
        public List<EmployeeAttendAbsenceAnalysisModel> resultModel { get; set; }


        public string startTime { get; set; }
        public string endTime { get; set; }
        public int shiftConceptDetailId { get; set; }
        public RollCallModelForEmployeeAttendAbsenceModel karKardCode { get; set; }
        public long entryId { get; set; }
        public long exitId { get; set; }
        public TimeSettingDataModel timeSettingDataModel { get; set; }
        public RollCallModelForEmployeeAttendAbsenceModel karKardCodeTomorrow { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public List<RollCallModelForEmployeeAttendAbsenceModel> karKardToOverTimeToday { get; set; }
        public List<RollCallModelForEmployeeAttendAbsenceModel> karKardToOverTimeTomorrow { get; set; }
    }
}
