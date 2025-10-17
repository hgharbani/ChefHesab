using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{

    public class EntryExitForOnCallSearchModel
    {
        public DateTime OnCallDate { get; set; }
        public int EmployeeId { get; set; }
        public int OnCallRequestId { get; set; }
    }

}
