
using Ksc.HR.DTO.ODSViews.ViewMisCostCenter;
using Ksc.HR.DTO.WorkShift.TeamWorkCategoryType;
using Ksc.HR.DTO.WorkShift.TeamWorkMangementCode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.TeamWorkCategory
{
    public class MISTeamWorkCategoryModel
    {


        [DisplayName("عنوان")]
        public string Title { get; set; } // Title (length: 200)


        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)

        [DisplayName("مرکز هزینه")]
        public decimal CostCenter { get; set; } // CostCenter (length: 50)

        [DisplayName("نوع گروه")]
        public int TeamWorkCategoryTypeId { get; set; } // TeamWorkCategoryTypeId

        [DisplayName("کد مدیریت")]
        public int TeamWorkMangementCodeId { get; set; } // TeamWorkMangementCode

        /// <summary>
        ///  فلگ گروه کاری
        ///1- ایجاد گروه 2- آپدیت گروه 3- حذف گروه	
        /// </summary>
        public string FLG_GRP_CRUD { get; set; }
        public string DomainName { get; set; }


    }
}
