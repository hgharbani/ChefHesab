using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.ODSViews.ViewMisEmploymentType
{
    public class SearchViewMisEmploymentTypeModel
    {
        [DisplayName("عنوان")]
        public string EmploymentTypeTitle { get; set; }  
        [DisplayName("کد")]
        public decimal? EmploymentTypeCode { get; set; }  
    }
}
