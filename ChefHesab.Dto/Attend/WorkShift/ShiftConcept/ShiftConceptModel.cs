using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.ShiftConcept
{
    public class ShiftConceptModel

    {
        public int Id { get; set; }
        [DisplayName("عنوان")]

        public string Title { get; set; } // Title (length: 200)

        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string DomainName { get; set; } // DomainName (length: 50)
        public bool IsActive { get; set; } // IsActive
        public bool IsRest { get; set; } // IsRest

    }
}
