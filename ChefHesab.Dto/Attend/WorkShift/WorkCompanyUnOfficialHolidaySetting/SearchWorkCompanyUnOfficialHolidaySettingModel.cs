using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.WorkCompanyUnOfficialHolidaySetting
{
    public class SearchWorkCompanyUnOfficialHolidaySettingModel
    {
        public int? Id { get; set; } // Id (Primary key)
        public int? WorkCompanySettingId { get; set; } // WorkCompanySettingId
        public int? WorkCalendarId { get; set; } // WorkCalendarId

        /// <summary>
        /// تعطیل است؟
        /// </summary>
        public bool? IsHoliday { get; set; } // IsHoliday

        /// <summary>
        /// دلیل تعطیلی
        /// </summary>
        public int? UnofficialHolidayReasonId { get; set; } // UnofficialHolidayReasonId

        public string UnofficialHolidayReasonTitle { get; set; } // UnofficialHolidayReasonId

        /// <summary>
        /// اضافه کار تعلق میگیرد؟
        /// </summary>
        public bool? IsValidExtraWork { get; set; } // IsValidExtraWork

        /// <summary>
        /// اضافه کاری برای تمام رده های شغلی معتبر است؟
        /// </summary>
        public bool? IsValidExtraWorkForAllCategoryCode { get; set; } // IsValidExtraWorkForAllCategoryCode
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string DomainName { get; set; } // DomainName (length: 50)
        public bool? IsActive { get; set; } // IsActive
    }
}
