using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.ShiftConceptDetail
{
    public class SearchShiftConceptDetailModel:FilterRequest
    {
        public int Id { get; set; } // Id (Primary key)
        [DisplayName("عنوان")]
        public string Title { get; set; } // Title (length: 200)
        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)
      
        public int WorkGroupId { get; set; }

        public int WorkCalendarId { get; set; }
        public int EmployeeId { get; set; }
        public int ShiftConceptId { get; set; }

    }
}
