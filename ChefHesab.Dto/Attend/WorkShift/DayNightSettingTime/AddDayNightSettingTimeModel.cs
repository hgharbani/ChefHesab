using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.DayNightSettingTime
{
    public class AddDayNightSettingTimeModel
    {
        public int Id { get; set; }

        [DisplayName("عنوان")]
        public string Title { get; set; } // Title (length: 200)
        public string DayStartTime { get; set; }
        public string DayEndTime { get; set; }
        public string NightStartTime { get; set; }
        public string NighEndTime { get; set; }

        public DateTime? InsertDate { get; set; } // InsertDate
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string CurrentUserName { get; set; }

        public string DomainName { get; set; } // DomainName (length: 50)

        public bool IsActive { get; set; } // IsActive
        public byte[] RowVersion { get; set; }





    }
}
