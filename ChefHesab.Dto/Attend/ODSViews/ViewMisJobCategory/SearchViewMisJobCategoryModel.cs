using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.ODSViews.ViewMisJobCategory
{
    public class SearchViewMisJobCategoryModel
    {
        [DisplayName("کد رده شغلی")]
        public decimal JobCategoryCode { get; set; } // 
        [DisplayName("عنوان رده شغلی")]
        public string JobCategoryTitle { get; set; } // 

        [DisplayName("کد دسته بندی رده شغلی")]
        public string CodeCategoryJobCategory { get; set; } // 

        public string ShowTitle { get; set; }
    }
}
