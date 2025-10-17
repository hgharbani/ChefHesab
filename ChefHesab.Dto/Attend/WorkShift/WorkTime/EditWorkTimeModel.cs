using Ksc.HR.DTO.WorkShift.ShiftConcept;
using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.WorkTime
{
    public class EditWorkTimeModel
    {
        public EditWorkTimeModel()
        {
            AvailbleWorkTimeCategori = new List<SearchWorkTimeCategoryModel>();
            AvailbleShiftConcept = new List<SearchShiftConceptModel>();
        }
        public int Id { get; set; } // Id (Primary key)

        [DisplayName("عنوان")]
        public string Title { get; set; } // Title (length: 200)

        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)
        [DisplayName("دسته بندی زمان کاری")]

        public int WorkTimeCategoryId { get; set; } // WorkTimeCategoryId

        [DisplayName("دوره تکرار")]
        public int RepetitionPeriod { get; set; } // RepetitionPeriod
        public DateTime? InsertDate { get; set; } // InsertDate
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string CurrentUserName { get; set; }
        public string DomainName { get; set; } // DomainName (length: 50)
        [DisplayName("فعال")]

        public bool IsActive { get; set; } // IsActive
        public List<SearchWorkTimeCategoryModel> AvailbleWorkTimeCategori { get; set; }
        [DisplayName("گروه کاری")]

        public List<string> WorkGroupTitle { get; set; }
        public List<string> LastWorkGroupTitle { get; set; }
        public List<string> LatinAlphabetList
        {
            get
            {
                return new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            }

        }
        public List<string> LatinAlphabetListValid { get; set; }
        public List<int> ShiftConceptId { get; set; }
        public List<SearchShiftConceptModel> AvailbleShiftConcept { get; set; }
        [DisplayName("چک کردن تنظیمات از لوحه شیفت باشد؟")]
        public bool ShiftSettingFromShiftboard { get; set; }
        [DisplayName("تعطیلات رسمی و غیر رسمی از تقویم کاری باشد؟")]
        public bool OfficialUnOfficialHolidayFromWorkCalendar { get; set; }
        [DisplayName("درصد شیفت کاری")]
        public double? PercentageWorkTime { get; set; }
        [DisplayName("حداکثر اضافه کاری قهری")]
        public string MaximumForcedOverTime { get; set; }
    }
}
