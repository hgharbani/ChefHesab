using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class EmployeeInfoModel
    {
        public string EmployeeNumber { get; set; }
        public int NumberEmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string ShiftTitle { get; set; }
        public string Address { get; set; }
        public int? EmploymentTypeId { get; set; }
        public string TeamWorkId { get; set; }
        public string TeamWorkTitle { get; set; }
        public string TeamWorkCode { get; set; }
        public int? JobPositionId { get; set; }
        public string JobPositionCode { get; set; }
        public decimal? CostCenter { get; set; }
        public int? WorkCityId { get; set; }
        public int? WorkGroupId { get; set; }
        public int WorktimeId { get; set; }
        public string WindowUser { get; set; }
    }
  
}
