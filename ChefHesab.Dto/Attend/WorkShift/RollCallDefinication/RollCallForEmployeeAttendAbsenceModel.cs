using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.RollCallDefinication
{
    public class RollCallForEmployeeAttendAbsenceModel
    {
        public List<RollCallModelForEmployeeAttendAbsenceModel> RollCallToday { get; set; }
        public List<RollCallModelForEmployeeAttendAbsenceModel> RollCallTomorrow { get; set; }
    }
}
