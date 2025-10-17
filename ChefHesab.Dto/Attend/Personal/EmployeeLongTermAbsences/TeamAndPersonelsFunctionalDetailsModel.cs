using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeLongTermAbsences
{
    public class TeamAndPersonelsFunctionalDetailsModel
    {
        //for personals

        [DisplayName("ساعت شروع شیفت")]
        public string ShiftStartTime { get; set; }
        
        [DisplayName("ساعت پایان شیفت")]
        public string ShiftEndTime { get; set; }
        
        [DisplayName("مانده مرخصی")]
        public string LeaveBalance { get; set; }
        [DisplayName("اضافه کار قهری")]
        public string ForcedOverTime { get; set; }
        [DisplayName("مجموع  سقف اضافه کاری ")]
        public string TotalCeilingOvertime { get; set; }
        [DisplayName("مجموع اضافه کاری جانبازی")]
        public string TotalVeteranOvertime { get; set; }
        [DisplayName("مجموع کل اضافه کاری")]
        public string TotalAllOvertime { get; set; }  //team

     //for team 
        [DisplayName("سقف اضافه کار")]
        public string OvertimeCeiling { get; set; }  //team
        [DisplayName("میانگین اضافه کار")]
        public string EmployeeAverageOvertime { get; set; }
        [DisplayName("تعداد کارکنان")]
        public string TeamEmployeeisCount { get; set; }
        [DisplayName("اضافه کار کارکنان")]
        public string EmployeeOvertime { get; set; } //team
        [DisplayName("میانگین کار کارکنان")]
        public string TeamAverageOvertime { get; set; } //team



        [DisplayName("مجموع  سقف اضافه کاری ")]
        public int TotalCeilingOvertimeDuration { get; set; } // person

        [DisplayName("سقف اضافه کار")]
        public int OvertimeCeilingDuration { get; set; }  //team

        public bool InValidOvertimeCeiling
        {
            get
            {
                var result = this.TotalCeilingOvertimeDuration > this.OvertimeCeilingDuration;
                return result;
            }
        }

    }
}
