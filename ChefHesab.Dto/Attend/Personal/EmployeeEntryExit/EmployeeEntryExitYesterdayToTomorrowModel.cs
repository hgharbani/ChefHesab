using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{

    public class EmployeeEntryExitYesterdayToTomorrowModel
    {
        public List<EmployeeEntryExitViewModel> YesterdayList { get; set; }
        public List<EmployeeEntryExitViewModel> TodayList { get; set; }
        public List<EmployeeEntryExitViewModel> TomorrowList { get; set; }
        public DateTime YesterdayDate { get; set; }
        public DateTime TomorrowDate { get; set; }
    }

}
