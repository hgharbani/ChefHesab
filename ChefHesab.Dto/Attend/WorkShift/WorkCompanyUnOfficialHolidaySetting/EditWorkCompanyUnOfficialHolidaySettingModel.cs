using Ksc.HR.DTO.ODSViews.ViewMisJobCategory;
using Ksc.HR.DTO.WorkShift.UnofficialHolidayReason;
using Ksc.HR.Resources.Workshift;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.WorkCompanyUnOfficialHolidaySetting
{
    public class EditWorkCompanyUnOfficialHolidaySettingModel
    {
        public EditWorkCompanyUnOfficialHolidaySettingModel()
        {
            AvailableCodeCategoryJobCategory = new List<SearchViewMisJobCategoryModel>();
            AvailableUnofficialHolidayReason = new List<SearchUnofficialHolidayReasonModel>();
            CodeCategoryJobCategorySelected = new List<string>();
        }

        public List<SearchViewMisJobCategoryModel> AvailableCodeCategoryJobCategory { get; set; }

        [Display(Name = nameof(CodeCategoryJobCategorySelected), ResourceType = typeof(WorkCompanyUnOfficialHolidaySettingResource))]
        public IEnumerable<string> CodeCategoryJobCategorySelected { get; set; }
        public List<SearchUnofficialHolidayReasonModel> AvailableUnofficialHolidayReason { get; set; }
        public string CurrentUserName { get; set; }
        public int Id { get; set; } // Id (Primary key)
        public int WorkCompanySettingId { get; set; } // WorkCompanySettingId


        [Display(Name = nameof(WorkCalendarId), ResourceType = typeof(WorkCompanyUnOfficialHolidaySettingResource))]
        public int WorkCalendarId { get; set; } // WorkCalendarId

        [Display(Name = nameof(WorkCalendarId), ResourceType = typeof(WorkCompanyUnOfficialHolidaySettingResource))]
        public DateTime? WorkCalendarDate { get; set; } // WorkCalendarId

        /// <summary>
        /// تعطیل است؟
        /// </summary>
        /// 
        [Display(Name = nameof(IsHoliday), ResourceType = typeof(WorkCompanyUnOfficialHolidaySettingResource))]
        public bool IsHoliday { get; set; } // IsHoliday

        /// <summary>
        /// دلیل تعطیلی
        /// </summary>
        /// 
        [Display(Name = nameof(UnofficialHolidayReasonId), ResourceType = typeof(WorkCompanyUnOfficialHolidaySettingResource))]
        public int? UnofficialHolidayReasonId { get; set; } // UnofficialHolidayReasonId

        [Display(Name = nameof(UnofficialHolidayReasonId), ResourceType = typeof(WorkCompanyUnOfficialHolidaySettingResource))]
        public string UnofficialHolidayReasonTitle { get; set; } // UnofficialHolidayReasonId

        /// <summary>
        /// اضافه کار تعلق میگیرد؟
        /// </summary>
        /// 
        [Display(Name = nameof(IsValidExtraWork), ResourceType = typeof(WorkCompanyUnOfficialHolidaySettingResource))]
        public bool IsValidExtraWork { get; set; } // IsValidExtraWork

        /// <summary>
        /// اضافه کاری برای تمام رده های شغلی معتبر است؟
        /// </summary>
        /// 
        [Display(Name = nameof(IsValidExtraWorkForAllCategoryCode), ResourceType = typeof(WorkCompanyUnOfficialHolidaySettingResource))]
        public bool IsValidExtraWorkForAllCategoryCode { get; set; } // IsValidExtraWorkForAllCategoryCode
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string DomainName { get; set; } // DomainName (length: 50)
        [Display(Name = nameof(IsActive), ResourceType = typeof(WorkCompanyUnOfficialHolidaySettingResource))]
        public bool IsActive { get; set; } // IsActive
    }
}
