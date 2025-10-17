using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.ODSViews.ViewMisSalaryCode
{
    public class SearchViewMisSalaryCodeModel
    {

        [DisplayName("کد محاسبه حقوق")]
        public decimal SalaryAccountCode { get; set; } // COD_ACN_ACNDF

        [DisplayName("عنوان کد محاسبه حقوق")]
        public string SalaryAccountTitle { get; set; } // DES_ACN_ACNDF (length: 60)
    }
}
