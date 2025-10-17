using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class EmployeeAttendAbcenseItemGroupModel
    {
        public int RollCallDefinitionId { get; set; }
        public int? RollCallCategoryId { get; set; }
        public double TotalDuration { get; set; }
    }
}
