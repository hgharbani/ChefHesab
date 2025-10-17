using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ksc.HR.DTO.Personal.EmployeeEfficiencyHistory
{
    public class EmployeeEfficiencyGridManageModel:FilterRequest
    {
        public int EmployeeId { get; set; }

        public string EmployeeNumber { get; set; }
        public int EmployeeNumber_Sort { get { return int.Parse(this.EmployeeNumber); }  }

        public string TeamCode { get; set; }
        public int TeamCode_Sort { get { return int.Parse(this.TeamCode); } }

        [DisplayName("ماه و سال")]
        public string YearMonth { get; set; }

        [DisplayName("نام و نام خانوادگی")]
        public string FullName { get; set; }

        [DisplayName("شماره تیم")]
        public string TeamTitle { get; set; }

        [DisplayName("ضریب کارایی جدید")]
        public decimal EfficiencyNew { get; set; }
        [DisplayName("ضریب کارایی قبلی")]
        public decimal EfficiencyOld { get; set; }

        [DisplayName("عنوان پست")]
        public string PostDesc { get; set; }

        [DisplayName("گروه شغل")]
        public int? GroupCode { get; set; }

        [DisplayName("نوع استخدام")]
        public int? EmploymentTypeId { get; set; }

        public bool IsActiveMonth { get; set; }

        public string CurrentUser { get; set; }
        public int Id { get; set; }


        public decimal? MinRange { get; set; }

        public decimal? MaxRange { get; set; }
        public int IsEffective { get; set; }

        public string RemoteIpAddress { get; set; }
        public bool IsChenged { get; set; }
    }
}
