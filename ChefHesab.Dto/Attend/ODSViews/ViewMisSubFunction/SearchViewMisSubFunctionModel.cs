using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.ODSViews.ViewMisSubFunction
{
    public class SearchViewMisSubFunctionModel
    {
        public decimal? MoavenatCode { get; set; } // MoavenatCode
        public string Moavenat { get; set; } // Moavenat (length: 60)

        public decimal? ManagmentCode { get; set; } // ManagmentCode
        public string Managment { get; set; } // Moavenat (length: 60)

        public decimal? SectionCode { get; set; } // ManagmentCode
        public string Section { get; set; } // Moavenat (length: 60)

        public decimal? CostCenterCode { get; set; } // CostCenterCode
        public decimal? EndDateSection { get; set; } // EndDateSection
        public int? IsActiveCostCenter { get; set; } //IsActiveCostCenter
    }
}
