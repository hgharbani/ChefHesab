using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.ODSViews.ViewMisJobPosition
{
    public class ViewMisJobPositionModel
    {
        public string JobPositionCode { get; set; } // JobPositionCode (length: 13)
        public string JobTitle { get; set; } // JobTitle (length: 50)
        public decimal? JobGroup { get; set; } // JobGroup
        public string SectionTitle { get; set; } // SectionTitle (length: 30)
        public string ManagmentTitle { get; set; } // ManagmentTitle (length: 30)
        public string AssistanceTilte { get; set; } // AssistanceTilte (length: 30)
        public decimal? JobPositionEndDate { get; set; } // JobPositionEndDate
        public decimal? ChartJobPositionEndDate { get; set; } // ChartJobPositionEndDate
        public decimal SubfunctionCode { get; set; } // SubfunctionCode
        public decimal? JobStatus { get; set; } // JobStatus
        public decimal ManagmentCode { get; set; } // ManagmentCode
        public decimal AssistanceCode { get; set; } // AssistanceCode
        public string JobCategoryCode { get; set; } // JobCategoryCode (length: 2)
        public decimal? JobStatusCode { get; set; } // JobStatusCode
        public string SuperiorJobPositionCode { get; set; } // SuperiorJobPositionCode (length: 13)
        public decimal CostCenterCode { get; set; } // CostCenterCode
    }
}
