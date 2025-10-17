using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ksc.HR.DTO.WorkShift.WorkTimeCategory
{
    public class AddOrEditWorkTimeCategoryModel
    {
        public int Id { get; set; }
        [DisplayName("عنوان")]
        [Required]
        public string Title { get; set; } // Title (length: 200)
        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)
        [DisplayName("تاریخ درج")]
        public DateTime? InsertDate { get; set; } // InsertDate
        [DisplayName("تاریخ ویرایش")]
        public DateTime? UpdateDate { get; set; } // UpdateDate

        public string CurrentUserName { get; set; }
        [DisplayName("نام دامنه")]
        public string DomainName { get; set; } // DomainName (length: 50)
        [DisplayName("فعال")]

        public bool IsActive { get; set; }

    }
}
