
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.WorkCalendar
{
    public class WorkCalendarModel
    {
        public int Id { get; set; }
        public string ShamsiDateV2 { get; set; }
        public DateTime MiladiDateV1 { get; set; }
        public int DayOfYear { get; set; }
        public int WeekOfYear { get; set; }

        public string WorkDayTypeCode { get; set; }
        public string WorkDayTypeTitle { get; set; }
        public string SystemSequenceStatusCode { get; set; }
        public string SystemSequenceStatusTitle { get; set; }
        public string HijriDateV1 { get; set; }
        public string HijriDateV2 { get; set; }
        public string MonthNameHijriV1 { get; set; }

    }
}
