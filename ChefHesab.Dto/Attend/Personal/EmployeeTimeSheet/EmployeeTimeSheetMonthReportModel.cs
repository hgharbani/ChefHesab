using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeTimeSheet
{
    public class EmployeeTimeSheetMonthReportModel
    {

        public string Name { get; set; }
        public string Family { get; set; }
        public string FullName { get; set; }

        public string DisplayMember { get { return $"{this.Name/*.Trim()*/} {this.Family/*.Trim()*/} "; } }
        /// <summary>
        /// کد تیم
        /// </summary>
        public string TeamCode { get; set; }//کد تیم

        /// <summary>
        /// نام تیم
        /// </summary>
        /// 
        public string TeamTitle { get; set; }//نام تیم
        public long EmployeeId { get; set; } // EmployeeId
        public string PersonalNumber { get; set; }

        public int YearMonth { get; set; } // YearMonth


        /// <summary>
        /// سقف اضافه کار
        /// </summary>
        public string MaximumDuration { get; set; }//سقف اضافه کار

        /// <summary>
        /// جمع اضافه کار
        /// </summary>
        public int CeilingOvertime { get; set; }//
        public string CeilingOvertime_Format { get; set; }//
                                                   

        /// <summary>
        /// اضافه کار کسر شده
        /// </summary>
        /// 
        public int ExcessOverTime { get; set; }//اضافهکار کسر شده

        /// <summary>
        /// اضافه کار کسر شده برای دقیقه و ساعت
        /// </summary>
        /// 
        public string ExcessOverTimeDuration { get; set; }//اضافهکار کسر شده برای دقیقه و ساعت

        public int ExcessOverTimeDuration_hour
        {
            get
            {
                return int.Parse(ExcessOverTimeDuration.Split(':')[0]);

            }
        }

        public int ExcessOverTimeDuration_minute
        {
            get
            {
                return int.Parse(ExcessOverTimeDuration.Split(':')[1]);

            }
        }

    }
}
