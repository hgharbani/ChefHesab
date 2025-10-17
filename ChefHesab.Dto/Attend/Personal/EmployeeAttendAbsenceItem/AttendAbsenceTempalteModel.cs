using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class AttendAbsenceTempalteModel
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RollCallDefinicationInItemModel RollCallDefinication { get; set; }
        public int RollCallConceptId { get; set; }
        public bool DeleteIsValid { get; set; }
        public bool ModifyIsValid { get; set; }
        public int ShiftConceptDetailId { get; set; }
        public long EntryId { get; set; }
        public long ExitId { get; set; }
        public string Duration { get; set; }
        public string OverTimeToken { get; set; }
        public bool TemprorayOverTimeInStartShift { get; set; }
        public bool TemprorayOverTimeInEndShift { get; set; }
    }
}
