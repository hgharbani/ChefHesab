using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.WorkTimeShiftConcept
{
    public class SearchWorkTimeShiftConceptModel
    {
        public int Id { get; set; }

        [DisplayName("زمان کاری")]
        public string WorkTimeTitle { get; set; }
        [DisplayName("شیفت کاری")]
        public string ShiftConceptTitle { get; set; }
        public string WorkTimeShiftConcepTitle { get; set; }
    }
}
