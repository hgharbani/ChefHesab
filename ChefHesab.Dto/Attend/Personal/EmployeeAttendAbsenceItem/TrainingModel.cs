using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class TrainingModel
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public RollCallModelForEmployeeAttendAbsenceModel RollCallTrainingOverTime { get; set; }
    }
}
