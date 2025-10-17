using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.WorkTimeCategory
{
    public class WorkTimeCategoryModel

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
        [DisplayName("تاریخ ویرایش")]
        public DateTime? UpdateDate { get; set; } // UpdateDate
        
        [DisplayName(" ویرایش کنندهکاربر")]
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        [DisplayName("نام دامنه")]
        public string DomainName { get; set; } // DomainName (length: 50)
        public bool IsActive { get; set; } // IsActive

    }
}
