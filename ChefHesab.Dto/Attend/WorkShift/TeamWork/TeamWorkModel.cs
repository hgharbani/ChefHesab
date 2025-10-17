
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.TeamWork
{
    public class TeamWorkModel
    {
        public string DisplayMember
        {
            get
            {
                return $"{Code}_{Title.Trim()}";
            }
        }

        public int Id { get; set; } // Id (Primary key)

        [DisplayName("عنوان تیم")]
        public string Title { get; set; } // Title (length: 500)

        [DisplayName("شماره تیم")]
        public string Code { get; set; } // Code (length: 50)

        [DisplayName("گروه تیم کاری")]
        public int? TeamWorkCategoryId { get; set; } // TeamWorkCategoryId

        [DisplayName("گروه تیم کاری")]
        public string TeamWorkCategoryTitle { get; set; } // TeamWorkCategoryId

        [DisplayName("میانگین و سقف اضافه کاری")]
        public int? OverTimeDefinitionId { get; set; } // OverTimeDefinitionId

        [DisplayName("میانگین و سقف اضافه کاری")]
        public string OverTimeDefinitionTitle { get; set; } // OverTimeDefinitionId


        [DisplayName("تاریخ شروع اعتبار")]
        public DateTime? ValidityStartDate { get; set; } // ValidityStartDate


        [DisplayName("تاریخ پایان اعتبار")]
        public DateTime? ValidityEndDate { get; set; } // ValidityEndDate


        [DisplayName("به این تیم کمد تعلق می گیرد؟")]
        public bool? HasCommode { get; set; } // HasCommode
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string DomainName { get; set; } // DomainName (length: 50)
        public bool IsActive { get; set; } // IsActive



    }
}
