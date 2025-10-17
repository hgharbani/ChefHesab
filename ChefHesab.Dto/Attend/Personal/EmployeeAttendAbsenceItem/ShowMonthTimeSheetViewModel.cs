using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class ShowMonthTimeSheetViewModel
    {
        /// <summary>
        /// کارکرد 
        /// </summary>
        public List<TimeSheetViewModel> TimeSheetViewModel { get; set; }

        public int MaxCountEntryExit { get; set; }
        /// <summary>
        /// اضافه کاری
        /// </summary>

        public string SumOverTime { get; set; }

        /// <summary>
        /// مرخصی ساعتی
        /// </summary>
        public string SumVacationPerHour { get; set; }
        /// <summary>
        /// مرخصی روزانه
        /// </summary>

        public string SumVacationPerDay { get; set; }

        /// <summary>
        /// غیبت ساعتی
        /// </summary>

        public string SumAbsencePerHour { get; set; }
        /// <summary>
        /// غیبت 
        /// </summary>
        public string SumAbsencePerDay { get; set; }
        /// <summary>
        /// غیبت
        /// </summary>
        public string AbsencePerHour { get; set; }

        public List<string> ReportMonth { get; set; }
        /// <summary>
        /// کوپن غذای استفاده شده
        /// </summary>
        public List<V_CouponType> V_CouponType { get; set; }
        /// <summary>
        /// کد تیم کاری
        /// </summary>
        public string TeamCode { get; set; }
        /// <summary>
        /// عنوان تیم کاری
        /// </summary>
        public string TeamTitle { get; set; }
        /// <summary>
        /// مسئول تیم کاری
        /// </summary>
        public string TeamSupervisor { get; set; }
    }

}
