using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.WorkTime
{
    public class WorkTimeModel
    {
        public WorkTimeModel()
        {
            AvailbleWorkTimeCategori = new List<SearchWorkTimeCategoryModel>();
        }
        public int Id { get; set; }

        [DisplayName("عنوان")] 
        public string Title { get; set; } // Title (length: 200)


        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)


        [DisplayName("عنوان")] 
        public int WorkTimeCategoryId { get; set; } // WorkTimeCategoryId

        [DisplayName("عنوان")] 
        public int RepetitionPeriod { get; set; } // RepetitionPeriod

        [DisplayName("تاریخ درج")] 
        public DateTime? InsertDate { get; set; } // InsertDate

        [DisplayName("کاربر")] 
        public string InsertUser { get; set; } // InsertUser (length: 50)

        [DisplayName("عنوان")] 
        public DateTime? UpdateDate { get; set; } // UpdateDate

        [DisplayName("عنوان")] 
        public string UpdateUser { get; set; } // UpdateUser (length: 50)

        [DisplayName("عنوان")] 
        public string DomainName { get; set; } // DomainName (length: 50)

        [DisplayName("عنوان")] 
        public bool IsActive { get; set; } // IsActive

        [DisplayName("دسته بندی زمان کاری")] 
        public string WorkTimeCategoryTitle { get; set; }

        [DisplayName("کد دسته بندی زمان کاری")] 
        public string WorkTimeCategoryCode { get; set; }

        [DisplayName("گروه کاری")]
        public string WorkGroupTitle { get; set; }
        public bool HasWorkTimeCategory { get; set; }
        [DisplayName("شیفت کاری")]
        public string ShiftConceptTitle { get; set; }
        public List<SearchWorkTimeCategoryModel> AvailbleWorkTimeCategori { get; set; }
        public bool ShiftSettingFromShiftboard { get; set; }
        public bool OfficialUnOfficialHolidayFromWorkCalendar { get; set; }
        public double? PercentageWorkTime { get; set; }
        public string MaximumForcedOverTime { get; set; }
    }
}
