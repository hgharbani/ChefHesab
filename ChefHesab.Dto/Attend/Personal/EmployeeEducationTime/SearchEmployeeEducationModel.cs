using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.EmployeeEducationTime
{
    public class SearchEmployeeEducationModel:FilterRequest
    {
        public int FromTeam { get; set; }
        public int ToTeam { get; set; }
        public int EmployeeId { get; set; }
        public bool IsOfficialAttendAbcense { get; set; }
        public string CurrentUserName { get; set; }
    }
}
