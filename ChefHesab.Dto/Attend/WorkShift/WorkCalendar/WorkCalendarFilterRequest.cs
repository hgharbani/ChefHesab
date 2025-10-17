using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.WorkCalendar
{
    public class WorkCalendarFilterRequest : FilterRequest
    {
        public string CalendarMonthPickerId { get;set;}
        public int FromYearmonth { get;set;}
        public int ToYearMonth { get;set; }
    }

}
