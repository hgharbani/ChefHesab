
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.TeamWorkCategory
{
    public class TeamWorkCategoryModel : FilterRequest
    {
        public int? Id { get; set; }

        [DisplayName("عنوان")] 
        public string Title { get; set; } // Title (length: 200)


        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)

        [DisplayName("مرکز هزینه")]
        public decimal CostCenter { get; set; } // CostCenter (length: 50)

        [DisplayName("مرکز هزینه")]
        public string CostCenterTitle { get; set; } 

        [DisplayName("نوع گروه")]
        public int TeamWorkCategoryTypeId { get; set; } // TeamWorkCategoryTypeId

        [DisplayName("نوع گروه")]
        public string TeamWorkCategoryTypeTitle { get; set; } 

        [DisplayName("کد مدیریت")]
        public int TeamWorkMangementCodeId { get; set; } // TeamWorkMangementCode

        [DisplayName("کد مدیریت")]
        public string TeamWorkMangementCodeTitle { get; set; } // TeamWorkMangementCode


        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string DomainName { get; set; } // DomainName (length: 50)
        [DisplayName("فعال")]
        public bool IsActive { get; set; } // IsActive
       

    }


    public class CostCenterDataForJoin
    {
        public decimal? CostCenterCode { get; set; }
        public string CostCenterTitle { get; set; }
        
            
    }
}
