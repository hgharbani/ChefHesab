using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class ShiftDetailModel
    {
        public int ShiftConceptId { get; set; }
        public int ShiftConceptDetailId { get; set; }
        public int WorkGroupId { get; set; }
        public int WorkTimeId { get; set; }
    }
}
