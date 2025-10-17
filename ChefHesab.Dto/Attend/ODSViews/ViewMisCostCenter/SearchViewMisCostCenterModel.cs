using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.ODSViews.ViewMisCostCenter
{
    public class SearchViewMisCostCenterModel
    {
        [DisplayName("عنوان")]
        public string CostCenterTitle { get; set; }  
        [DisplayName("کد")]
        public decimal? CostCenterCode { get; set; }  
    }
}
