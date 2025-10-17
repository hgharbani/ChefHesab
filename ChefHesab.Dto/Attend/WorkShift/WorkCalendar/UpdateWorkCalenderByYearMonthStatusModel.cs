
using Ksc.HR.Resources.Workshift;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.WorkCalendar
{
    public class UpdateWorkCalenderByYearMonthStatusModel
    {
        public string Yearmonth { get; set; }
        //[Required(ErrorMessageResourceName = "RequiredSystemSequenceStatusIdAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Workshift.WorkCalendarResource))]
        //[Display(Name = nameof(SystemSequenceStatusId), ResourceType = typeof(WorkCalendarResource))]
        public string SystemSequenceStatusId { get; set; }
        public string CurrentUser { get; set; }
        public string Step { get; set; }
    }
}
