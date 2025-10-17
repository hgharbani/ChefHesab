using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.RollCallDefinication
{
    public class SearchRollCallDefinicationModel : FilterRequest
    {
        public SearchRollCallDefinicationModel()
        {
           
        }
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } // Title (length: 500)

        public string ShowCodeTitle { get { return Code + "-" + Title; } }
        /// <summary>
        /// کد
        /// </summary>
        public string Code { get; set; } // Code (length: 50)

        public int Total { get; set; }
        /// <summary>
        /// تاریخ شروع اعتبار
        /// </summary>
        public DateTime? ValidityStartDate { get; set; } // ValidityStartDate

        /// <summary>
        /// تاریخ پایان اعتبار
        /// </summary>
        public DateTime? ValidityEndDate { get; set; } // ValidityEndDate
        public int? RollCallCategoryId { get; set; }
        public int? ShiftConceptDetailId { get; set; }
        /// <summary>
        /// نوع استخدام دسترسی به کد
        /// </summary>
        public int? EmploymentTypeCodeId { get; set; }
        public bool IsValidForTemporaryStartDate { get; set; }
        public bool IsValidForTemporaryEndDate { get; set; }
        public bool IsIncluded { get; set; }
        public bool LongTermAbsenceCheck { get; set; }
        public int RollCallConceptId { get; set; }
    }
}
