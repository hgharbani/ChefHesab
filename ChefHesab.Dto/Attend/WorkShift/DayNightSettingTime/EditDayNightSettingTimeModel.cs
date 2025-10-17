using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.DayNightSettingTime
{
    public class EditDayNightSettingTimeModel
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
        public bool IsActive { get; set; } // IsActive


    }
}
