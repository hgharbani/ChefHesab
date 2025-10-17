using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{

    public class EntryExitResult
    {
       // public string Entry { get; set; }//EntryExitTime (length: 5)
       // public string Exit { get; set; }// EntryExitTime (length: 5)
        public string ExitDateString { get; set; } // EntryExitDate
        public DateTime? ExitDate { get; set; } // EntryExitDate miladi
        public string EntryDateString { get; set; } // EntryDateString
        public DateTime? EntryDate { get; set; } // EntryDate miladi

        public string EntryTime { get; set; } // EntryExitTime (length: 5)
        public string ExitTime { get; set; } // EntryExitTime (length: 5)
        public bool Selected { get; set; }
        public int WfRequestId { get; set; }
        public long Id { get; set; }

        public string CurrentUserName { get; set; }
        public string DomainName { get; set; }
    }

}
