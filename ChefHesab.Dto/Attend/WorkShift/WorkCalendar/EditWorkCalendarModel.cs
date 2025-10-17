
using Ksc.HR.Resources.Workshift;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.WorkCalendar
{
    public class EditWorkCalendarModel
    {
        //public WorkCalendar()
        //{
        //    ShiftBoard = new HashSet<ShiftBoard>();
        //}

        public int Id { get; set; }
        //public int DateKey { get; set; }
        //public int DayOfYear { get; set; }
        //public int DayOfWeek { get; set; }
        //public int WeekOfYear { get; set; }
        //public int Month { get; set; }
        //public int YearMonthV1 { get; set; }
        //public string YearMonthV2 { get; set; }
        //public string SeasonName { get; set; }
        //public int SeasonNumber { get; set; }
        //public string HalfYearName { get; set; }
        //public int HalfYearNumber { get; set; }
        //public string ShamsiDateV1 { get; set; }
        //public string ShamsiDateV2 { get; set; }
        //public DateTime MiladiDateV1 { get; set; }
        //public string MiladiDateV2 { get; set; }
        //public string HijriDateV1 { get; set; }
        //public string HijriDateV2 { get; set; }
        //public string DayNameShamsi { get; set; }
        //public string DayNameMiladi { get; set; }
        //public string DayNameHijri { get; set; }
        //public string MonthNameShamsiV1 { get; set; }
        //public string MonthNameMiladiV1 { get; set; }
        //public string MonthNameMiladiV2 { get; set; }
        //public string MonthNameHijriV1 { get; set; }
        //public int DayOfMonthShamsi { get; set; }
        //public int DayOfMonthHijri { get; set; }
        //public int DayOfMonthMiladi { get; set; }
        //public string Mmddshamsi { get; set; }
        //public string Mmddmiladi { get; set; }
        //public string Mmddhijri { get; set; }
        //public int Yyyymmshamsi { get; set; }
        //public int Yyyymmmiladi { get; set; }
        //public int Yyyymmhijri { get; set; }
        //public int Yyyyshamsi { get; set; }
        //public int Yyyyhijri { get; set; }
        //public int Yyyymiladi { get; set; }
        [Required(ErrorMessageResourceName = "RequiredWorkDayTypeIdAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Workshift.WorkCalendarResource))]
        [Display(Name = nameof(WorkDayTypeId), ResourceType = typeof(WorkCalendarResource))]
        public int? WorkDayTypeId { get; set; }
        [Required(ErrorMessageResourceName = "RequiredSystemSequenceStatusIdAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Workshift.WorkCalendarResource))]
        [Display(Name = nameof(SystemSequenceStatusId), ResourceType = typeof(WorkCalendarResource))]
        public int? SystemSequenceStatusId { get; set; }
        //public byte[] RowVersion { get; set; }

        //public virtual TimeSheetStatus TimeSheetStatus { get; set; }
        //public virtual WorkDayType WorkDayType { get; set; }
        //public virtual ICollection<ShiftBoard> ShiftBoard { get; set; }

    }
    public class EditWorkCalenderStatus
    {
        public string Ids { get; set; }
        [Required(ErrorMessageResourceName = "RequiredSystemSequenceStatusIdAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Workshift.WorkCalendarResource))]
        [Display(Name = nameof(SystemSequenceStatusId), ResourceType = typeof(WorkCalendarResource))]
        public int SystemSequenceStatusId { get; set; }
    }
   
}
