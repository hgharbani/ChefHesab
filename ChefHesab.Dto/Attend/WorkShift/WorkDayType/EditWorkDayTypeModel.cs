using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.WorkDayType
{
    public class EditWorkDayTypeModel
    {
        public int Id { get; set; } // Id (Primary key)
        [DisplayName("عنوان")]
        public string Title { get; set; } // Title (length: 200)
        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)
        public DateTime? InsertDate { get; set; } // InsertDate
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string CurrentUserName { get; set; }
        public string DomainName { get; set; } // DomainName (length: 50)
        [DisplayName("فعال")]
        public bool IsActive { get; set; } // IsActive

        [DisplayName("قابل استفاده در کدهای حضور غیاب")]
        public bool UseInRollCall { get; set; } // UseInRollCall
        [DisplayName("تعطیل است؟")]
        public bool IsHoliday { get; set; } // IsHoliday
        [DisplayName("تعطیل رسمی کشوری است؟")]
        public bool IsOfficialHoliday { get; set; } // IsOfficialHoliday
    }
}
