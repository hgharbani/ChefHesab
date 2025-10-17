using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{

    public class EmployeeEntryExitViewModel
    {
        public long EntryId { get; set; }
        public long ExitId { get; set; }
        public string EntryTime { get; set; } // EntryExitTime (length: 5)
        public string ExitTime { get; set; } // EntryExitTime (length: 5)
        public TimeSpan EntryTimeToTimeSpan { get; set; } // EntryExitTime (length: 5)
        public TimeSpan ExitTimeToTimeSpan { get; set; } // EntryExitTime (length: 5)
        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }
        public bool IsCreatedManual { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? EntryUpdateDate { get; set; }
        public string EntryUpdateUser { get; set; }
        public DateTime? ExitUpdateDate { get; set; }
        public string ExitUpdateUser { get; set; }
        public bool EntryIsCreatedManual { get; set; }//دستی I
        public bool EntryIsDeleted { get; set; }
        public bool ExitIsCreatedManual { get; set; }//دستی I
        public bool ExitIsDeleted { get; set; }
        public Guid RowGuid { get; set; }
        public int EmployeeId { get; set; }
    }

}
