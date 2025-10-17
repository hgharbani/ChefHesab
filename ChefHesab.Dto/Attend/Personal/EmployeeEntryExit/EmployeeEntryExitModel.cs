using Ksc.HR.Resources.Personal;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.General;


namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{
    public class EmployeeEntryExitModel
    {
        public long Id { get; set; } // Id (Primary key)
        public string PersonalNumber { get; set; } // PersonalNumber (length: 10)
        public string FullName { get; set; } // PersonalNumber (length: 10)
        public DateTime EntryExitDate { get; set; } // EntryExitDate
        public string EntryExitTime { get; set; } // EntryExitTime (length: 5)
        public TimeSpan? EntryExitTimeSpan { get {
                return
                    !string.IsNullOrEmpty(EntryExitTime) ? EntryExitTime.ConvertStringToTimeSpan() : null;


                    } } // EntryExitTime (length: 5)

        public int? EntryExitNumber
        {
            get
            {
                return
                    !string.IsNullOrEmpty(EntryExitTime) ? int.Parse(EntryExitTime.Replace(":","")): null;


            }
        } // EntryExitTime (length: 5)
        public int EntryExitType { get; set; } // EntryExitType
        public string MachinName { get; set; } // MachinName (length: 2)
        public bool IsCreatedManual { get; set; } // IsCreatedManual
        public DateTime? CreateDateTime { get; set; } // CreateDateTime
        public string CreateUser { get; set; } // CreateUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public DateTime? DeletedDate { get; set; } // DeletedDate
        public string DeletedUser { get; set; } // DeletedUser (length: 50)
        public bool IsDeleted { get; set; } // IsDeleted
        public DateTime? EntryDate { get; set; }
        public string EntryDateString { get; set; }
        public DateTime? ExitDate { get; set; } // EntryExitDate
        public string ExitDateString { get; set; } // EntryExitDate
        public string EntryTime { get; set; } // EntryExitTime (length: 5)
        public string ExitTime { get; set; } // EntryExitTime (length: 5)
        public bool Selected { get; set; }
        public string EntryExitTypeString { get; set; }
        public string AssistanceTitle { get; set; }
        public string ManagementTitle { get; set; }
        public string WorkTimeTitle { get; set; }
    }

    //public class EmployeeEntryExitListModel
    //{
    //    public int Id { get; set; }
    //    public DateTime EntryDate { get; set; } // EntryExitDate
    //    public string EntryTime { get; set; } // EntryExitTime (length: 5)
    //    public DateTime ExitDate { get; set; } // EntryExitDate
    //    public string ExitTime { get; set; } // EntryExitTime (length: 5)
    //    public int EmployeeEntryExitId { get; set; } // Id (Primary key)
    //    public bool Selected { get; set; }

    //}

}
