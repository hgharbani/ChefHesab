using Ksc.HR.DTO.WorkShift.ShiftConcept;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.ShiftConceptDetail
{
    public class ShiftConceptDetailModel
    {
        public ShiftConceptDetailModel()
        {
            AvailableShiftConcept = new List<SearchShiftConceptModel>();
        }
        public int Id { get; set; } // Id (Primary key)
        [DisplayName("عنوان")]
        public string Title { get; set; } // Title (length: 200)
        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)
        [DisplayName("ماهیت شیفت")]
        public int ShiftConceptId { get; set; } // ShiftConceptId
        public bool OncallCheckBeforeShiftStart { get; set; } // OncallCheckBeforeShiftStart
        public bool OncallCheckAfterShiftEnd { get; set; } // OncallCheckAfterShiftEnd
        public bool OncallCheckAfterShiftStart { get; set; } // OncallCheckAfterShiftStart
        public bool OncallCheckFree { get; set; } // OncallCheckFree
        /// <summary>
        /// مدت زمان قبل از شروع شیفت
        /// </summary>
        public string DurationTimeBeforeShiftStartTime { get; set; } // DurationTimeBeforeShiftStartTime (length: 5)

        /// <summary>
        /// مدت زمان بعد از پایان شیفت
        /// </summary>
        public string DurationTimeAfterShiftEndTime { get; set; } // DurationTimeAfterShiftEndTime (length: 5)
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public string DomainName { get; set; } // DomainName (length: 50)
        public bool IsActive { get; set; } // IsActive

        public List<SearchShiftConceptModel> AvailableShiftConcept { get; set; }

        public string ShiftConceptTitle { get; set; }

        public string ShiftConceptCode { get; set; }

        public string ShowCodeDescription { get { return Code + "-" + Title; } }
       
    }
}
