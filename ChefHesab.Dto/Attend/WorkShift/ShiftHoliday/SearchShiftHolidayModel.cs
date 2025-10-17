using Ksc.HR.DTO.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.ShiftHoliday
{
    public class SearchShiftHolidayModel
    {
        public int Id { get; set; }
        public int WorkCompanySettingId { get; set; }
        public List<SelectListItem> AvailbleDayNumberTypes { get; set; }

    }
}
