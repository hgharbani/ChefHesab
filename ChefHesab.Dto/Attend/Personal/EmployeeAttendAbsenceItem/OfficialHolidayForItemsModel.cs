using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class OfficialHolidayForItemsModel
    {
        

        /// <summary>
        /// تاریخ 
        /// </summary>
      

        public DateTime? HolidayDate { get; set; }
        public string CurrentUserName { get; set; }
        public bool IsValidOfficial { get; set; }

        

    }
}
