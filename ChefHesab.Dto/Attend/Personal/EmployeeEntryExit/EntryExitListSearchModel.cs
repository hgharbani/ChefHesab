using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{

    public class EntryExitListSearchModel
    {
        public DateTime entryExitDate { get; set; }
        public string person { get; set; }
        public int WfRequestId { get; set; }
    }

}
