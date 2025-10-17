using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{

    public class EntryExitList
    {
        public int type { get; set; }
        public string time { get; set; }
        public DateTime EntryExitDate { get; set; }
        public int WorkCalendarId { get; set; }
        public int EmployeeId { get; set; }
        public long Id { get; set; }
        public bool EntryIsCreatedManual { get; set; }//دستی I
        public bool EntryIsDeleted { get; set; }
        public bool ExitIsCreatedManual { get; set; }//دستی I
        public bool ExitIsDeleted { get; set; }
        public bool IsCreatedManual { get; set; }//دستی I
        public bool IsDeleted { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        
    }

}
