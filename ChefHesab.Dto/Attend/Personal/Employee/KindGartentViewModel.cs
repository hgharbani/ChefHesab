using Ksc.HR.DTO.Emp.Family;
using KSCCommunicationAPI.Models.Class.ADManagementApiClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class KindGartentViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string FullName { get; set; }
        public int ChildrenCount { get; set; }

        public long Amount { get; set; }
        public long BaseAmount { get; set; }
        public int DaysWork { get; set; }
        public int karkard { get; set; }

        public bool IsActive { get; set; }
        public int AccountCode { get; set; }

        public bool IsDeleted { get; set; }

        public List<string> EmployeeIds { get; set; }

    }



}
