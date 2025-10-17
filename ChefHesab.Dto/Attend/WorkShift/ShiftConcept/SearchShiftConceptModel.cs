using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.ShiftConcept
{
    public class SearchShiftConceptModel
    {
        public int Id { get; set; }

        [DisplayName("عنوان")]
        public string Title { get; set; } // Title (length: 200)
        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)
        public string WorkGroupCode { get; set; }
        public bool? IsRest { get; set; }

        public string ShiftConceptCode { get; set; }
        public int ShiftConceptId { get; set; }
       //public bool IsRestShift { get; set; }
    }
}
