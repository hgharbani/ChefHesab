using Ksc.HR.DTO.WorkShift.ShiftConcept;
using Ksc.HR.Resources.Workshift;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.ShiftConceptDetail
{
    public class EditShiftConceptDetailModel
    {
        public EditShiftConceptDetailModel()
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
        [Display(Name = "OncallCheckBeforeShiftStart", Description = "OncallCheckBeforeShiftStart", ResourceType = typeof(ShiftConceptDetailResource))]
        public bool OncallCheckBeforeShiftStart { get; set; } // OncallCheckBeforeShiftStart
        [Display(Name = "OncallCheckAfterShiftEnd", Description = "OncallCheckAfterShiftEnd", ResourceType = typeof(ShiftConceptDetailResource))]
        public bool OncallCheckAfterShiftEnd { get; set; } // OncallCheckAfterShiftEnd
        [Display(Name = "OncallCheckAfterShiftStart", Description = "OncallCheckAfterShiftStart", ResourceType = typeof(ShiftConceptDetailResource))]
        public bool OncallCheckAfterShiftStart { get; set; } // OncallCheckAfterShiftStart
        [Display(Name = "OncallCheckFree", Description = "OncallCheckFree", ResourceType = typeof(ShiftConceptDetailResource))]
        public bool OncallCheckFree { get; set; } // OncallCheckFree
        [Display(Name = "DurationTimeBeforeShiftStartTime", Description = "DurationTimeBeforeShiftStartTime", ResourceType = typeof(ShiftConceptDetailResource))]
        public string DurationTimeBeforeShiftStartTime { get; set; } // DurationTimeBeforeShiftStartTime (length: 5)
        [Display(Name = "DurationTimeAfterShiftEndTime", Description = "DurationTimeAfterShiftEndTime", ResourceType = typeof(ShiftConceptDetailResource))]
        public string DurationTimeAfterShiftEndTime { get; set; } // DurationTimeAfterShiftEndTime (length: 5)
        public DateTime? InsertDate { get; set; } // InsertDate
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string CurrentUserName { get; set; }
        public string DomainName { get; set; } // DomainName (length: 50)
        [DisplayName("فعال")]

        public bool IsActive { get; set; } // IsActive

        public List<SearchShiftConceptModel> AvailableShiftConcept { get; set; }

    }
}
