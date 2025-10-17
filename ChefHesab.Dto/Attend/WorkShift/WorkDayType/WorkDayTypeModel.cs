using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.WorkDayType
{
    public class WorkDayTypeModel
    {
        public int Id { get; set; } // Id (Primary key)
        public string Title { get; set; } // Title (length: 200)
        public string Code { get; set; } // Code (length: 50)
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string DomainName { get; set; } // DomainName (length: 50)
        public bool IsActive { get; set; } // IsActive
        public bool UseInRollCall { get; set; } // UseInRollCall
        public bool IsHoliday { get; set; } // IsHoliday
        public bool IsOfficialHoliday { get; set; } // IsOfficialHoliday
    }
}
