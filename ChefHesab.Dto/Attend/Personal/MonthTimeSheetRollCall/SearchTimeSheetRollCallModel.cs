using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Resources.Personal;
namespace Ksc.HR.DTO.Personal.MonthTimeSheetRollCall
{
    public class SearchTimeSheetRollCallModel : FilterRequest
    {
        public string YearMonth { get; set; }
        public int EmployeeId { get; set; }
        public bool IsActive { get; set; }
        /// <summary>
        /// کد حضور وغیاب
        /// </summary>
        [Display(Name = nameof(Code), ResourceType = typeof(MonthTimeSheetRollCallResource))]

        public string Code { get; set; }
        /// <summary>
        /// شرح
        /// </summary>
        [Display(Name = nameof(Title), ResourceType = typeof(MonthTimeSheetRollCallResource))]
        public string Title { get; set; }
        /// <summary>
        /// روز
        /// </summary>
        [Display(Name = nameof(DayCountInDailyTimeSheet), ResourceType = typeof(MonthTimeSheetRollCallResource))]
        public int DayCountInDailyTimeSheet { get; set; }
        /// <summary>
        /// ساعت
        /// </summary>
        [Display(Name = nameof(Duration), ResourceType = typeof(MonthTimeSheetRollCallResource))]
        public string Duration { get; set; }
        /// <summary>
        /// دقیقه
        /// </summary>
        //public int DurationInMinut { get; set; }
        /// <summary>
        /// کد پرداخت
        /// </summary>
        [Display(Name = nameof(AccountCode), ResourceType = typeof(MonthTimeSheetRollCallResource))]
        public string AccountCode { get; set; }
        /// <summary>
        /// شرح پرداخت
        /// </summary>
        [Display(Name = nameof(AccountTitle), ResourceType = typeof(MonthTimeSheetRollCallResource))]
        public string AccountTitle { get; set; }
        public int RollCallDefinitionId { get; set; }
        public int DurationInMinut { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        /// <summary>
        /// اضافه کار سقف هستند
        /// </summary>
        public bool RollCallDefinitionInCeilingOvertime { get; set; }
    }
}
