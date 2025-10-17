using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{
    public class EmployeeEntryExitForAttendAbsenceItemModel
    {
        public DateTime EntryExitDate { get; set; }
        public int EmployeeId { get; set; }
        public List<EmployeeEntryExitViewModel> EmployeeEntryExitViewModel { get; set; }
  

    }
}
