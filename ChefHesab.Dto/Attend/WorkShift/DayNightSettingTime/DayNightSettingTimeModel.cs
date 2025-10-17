using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkShift.Province;

namespace Ksc.HR.DTO.WorkShift.DayNightSettingTime
{
    public class DayNightSettingTimeModel
    {
       
        public int Id { get; set; }

        [DisplayName("عنوان")] 
        public string Title { get; set; } // Title (length: 200)


        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)


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
        public string ProvinceTitle { get; set; }

        [DisplayName("کد دسته بندی زمان کاری")] 
        public string ProvinceCode { get; set; }
    
    }
}
