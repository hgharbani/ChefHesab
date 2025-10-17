using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ksc.HR.DTO.Personal.EmployeeTimeSheet
{
    public class EmployeeTimeSheetGridManageModel
    {
        public int EmployeeId { get; set; }
        public int EmployeeTimeSheetId { get; set; }
        public string EmployeeNumber { get; set; }
        [DisplayName("ماه و سال")]

        public string YearMonth { get; set; }
        [DisplayName("نام و نام خانوادگی")]
        public string FullName { get; set; }
        [DisplayName("شماره تیم")]
        public string TeamCode { get; set; }

        [DisplayName("مشمول سقف")]
        public float SumOverTimeLimit { get; set; }

        [DisplayName("مشمول میانگین")]
        public float SumOverTimeAverage { get; set; }

        [DisplayName("مجموع اضافه کار")]
        public float SumOverTime { get; set; }

        [DisplayName("تعدیل میانگین")]
        public int? AverageBalanceOverTime { get; set; }
        public bool IsActiveMonth { get; set; }
        public string AverageBalanceOverTimeDuration { get; set; }
        public string CurrentUser { get; set; }
        public int Id { get; set; }
        public int SumForeOverTime { get; set; }
    }
}
