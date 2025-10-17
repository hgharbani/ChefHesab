using System;

namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{
    public class JobCategorySpecificEntryExitModel
    {
        public long EntryExitId { get; set; }

        public int WorkCalendarId { get; set; }
        public string EntryExitTime { get; set; }
        public int EntryExitTimeType { get; set; }
        public string EmployeeId { get; set; }
        //public bool IsManual { get; set; }
        public bool? IsCreatedManual { get; set; }//دستی I
        public bool IsDeleted { get; set; }
        public bool? EntryIsCreatedManual { get; set; }//دستی I
        public bool EntryIsDeleted { get; set; }
        public bool? ExitIsCreatedManual { get; set; }//دستی I
        public bool ExitIsDeleted { get; set; } 

        public DateTime EntryUpdateDate { get; set; }
        public string EntryUpdateUser { get; set; }
        public DateTime ExitUpdateDate { get; set; }
        public string ExitUpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string RemoteIpAddress { get; set; }
    }

}
