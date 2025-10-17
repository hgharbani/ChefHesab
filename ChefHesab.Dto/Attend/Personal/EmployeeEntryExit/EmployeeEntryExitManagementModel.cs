using Ksc.HR.DTO.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{

    public class EmployeeEntryExitManagementModel
    {
        public EmployeeEntryExitManagementModel()
        {
            EntryExistTypeList = new List<SelectListItem>();
            EntryExistTypeList.Add(new SelectListItem { Text = "ورود", Value = "1" });
            EntryExistTypeList.Add(new SelectListItem { Text = "خروج", Value = "2" });
        }
        public long Id { get; set; } // Id (Primary key)
        public string EntryExitTime { get; set; } // EntryExitTime (length: 5)
        public int EntryExitType { get; set; } // EntryExitType
        public bool IsCreatedManual { get; set; } // IsCreatedManual
        public bool IsDeleted { get; set; } // IsDeleted
        public bool IsCanBeInsert { get; set; } // برای چک ثبت ورود خروج
     
        public List<SelectListItem> EntryExistTypeList { get; set; }
        public string CurrentUserName { get; set; } // CurrentUserName (length: 5)
        public string AuthenticateUserName { get; set; } 

        public string CurrentDomain { get; set; } // CurrentDomain (length: 5)

        public int EmployeeId { get; set; }
        public string EntryExistDate { get; set; }
        public string EntryExitTypeTitle { get; set; }
        public string UpdateUser { get; set; }

        public string DeletedUser { get; set; }
        public string RemoteIpAddress { get; set; }



    }
}
