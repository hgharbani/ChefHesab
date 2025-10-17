using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{
    public class EditEmployeeEntryExitModel
    {

        public int Id { get; set; } // Id (Primary key)
        public string PersonalNumber { get; set; } // PersonalNumber (length: 10)
        public DateTime EntryExitDate { get; set; } // EntryExitDate
        public string EntryExitTime { get; set; } // EntryExitTime (length: 5)
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


    }
}
