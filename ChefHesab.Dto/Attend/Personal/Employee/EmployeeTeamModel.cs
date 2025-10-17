using Ksc.HR.DTO.WorkShift.ShiftConcept;
using Ksc.HR.Resources.Personal;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class EmployeeTeamModel : FilterRequest
    {
        public int Id { get; set; }
        [Display(Name = nameof(EmployeeNumber), ResourceType = typeof(EmployeeResource))]
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)

        [Display(Name = nameof(Name), ResourceType = typeof(EmployeeResource))]
        public string Name { get; set; } // Name (length: 500)

        [Display(Name = nameof(FullName), ResourceType = typeof(EmployeeResource))]
        public string FullName { get { return Name + " " + Family; } }

        [Display(Name = nameof(Family), ResourceType = typeof(EmployeeResource))]
        public string Family { get; set; } // Family (length: 500)
        public string TeamWorkCode { get; set; }//تیم کاری
        public string TeamWorkId { get; set; }//تیم کاری
        public int EmployeeId { get; set; }
        public int ShiftConceptDetailsId { get; set; }
        public SearchShiftConceptModel ShiftCode { get; set; }
        /// <summary>
        /// عنوان تیم کاری
        /// </summary>
        public string TeamWorkTitle { get; set; }
        /// <summary>
        /// زمان کاری
        /// </summary>
        public string WorkGroupWorkTimeTitle { get; set; }
        /// <summary>
        /// کد گروه کاری
        /// </summary>
        public string WorkGroupCode { get; set; }
        public DateTime? WorkGroupStartDate { get; set; }
        public DateTime? TeamWorkStartDate { get; set; }
        public bool IsOfficialAttendAbcense { get; set; }
        public string CurrentUserName { get; set; }
        public DateTime TeamStartDate { get; set; }
        public DateTime? TeamEndDate { get; set; }
        public bool IsActive { get; set; }
        public int? PaymentStatusId { get; set; }
    }
}
