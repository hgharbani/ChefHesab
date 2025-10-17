
using Ksc.HR.DTO.WorkShift.OverTimeDefinition;
using Ksc.HR.DTO.WorkShift.TeamWorkCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.TeamWork
{
    public class MISTeamWorkModel
    {

        [DisplayName("عنوان تیم")]
        public string Title { get; set; } // Title (length: 500)

        [DisplayName("شماره تیم")]
        public string Code { get; set; } // Code (length: 50)

        [DisplayName("گروه")]
        public int TeamWorkCategoryId { get; set; } // TeamWorkCategoryId

        [DisplayName("میانگین و سقف اضافه کاری")]
        public int? OverTimeDefinitionId { get; set; } // OverTimeDefinitionId

        [DisplayName("تاریخ شروع اعتبار")]
        public DateTime? ValidityStartDate { get; set; } // ValidityStartDate


        [DisplayName("تاریخ پایان اعتبار")]
        public DateTime? ValidityEndDate { get; set; } // ValidityEndDate


        /// <summary>
        ///  فلگ گروه کاری
        ///1-ایجاد تیم 2- ویرایش تیم 3-حذف تیم	
        /// </summary>
        public string FLG_TEAM_CRUD  { get; set; }
        public string DomainName  { get; set; }

    }
}
