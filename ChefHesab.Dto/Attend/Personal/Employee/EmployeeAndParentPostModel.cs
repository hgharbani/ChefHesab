using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class EmployeeAndParentPostModel
    {
        public int EmployeeNumber { get; set; }
        public string FullName { get; set; }

        public int ParentEmployeeNumber { get; set; }
        public string ParentFullName { get; set; }      
        public string ParentUserWindow { get; set; }
    }
}
