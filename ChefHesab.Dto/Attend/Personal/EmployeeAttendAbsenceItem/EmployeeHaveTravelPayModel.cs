using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class EmployeeHaveTravelPayParentModel
    {
        public EmployeeHaveTravelPayParentModel()
        {
            model = new List<EmployeeHaveTravelPayModel>();
        }
        public int CountList { get; set; }
        public List<EmployeeHaveTravelPayModel> model { get; set; }
    }
    public class EmployeeHaveTravelPayModel
    {
        public string PersonnelNumber { get; set; }
        public string Datetime { get; set; }
        public int EmployeeId { get; set; }
        public int CountLeve { get; set; }
    }
}
