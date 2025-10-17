using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class EmployeeDataForOtherSystemModel
    {
        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string NationalCode { get; set; }
    }
}
