using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeTimeSheet
{
    public class EmployeeCompensatoryOverTimeModel
    {
        public int YearMonth { get; set; }
        public string CurrentUser { get; set; }
        public string MaximumOverTimeDuration { get; set; }
    }
}
