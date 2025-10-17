
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.WorkCalendar
{
    public class WorkCalendarIsHolidayModel
    {
        public bool IsHoliday { get; set; }
        public string ShamsiDateV2 { get; set; }
        public string ShamsiDateV1 { get; set; }
        public DateTime MiladiDateV1 { get; set; }
        public int DayOfWeek { get; set; }
        public string WorkDayTypeTitle { get; set; }
        public string HijriDateV1 { get; set; }
        public int DayOfYear { get; set; }
    }
}
