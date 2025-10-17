using Ksc.HR.DTO.WorkShift.TeamWork;
using Ksc.HR.DTO.WorkShift.WorkGroup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class EmployeeTeamManagmentModel
    {
        // <summary>
        // فهرست پرسنل
        // </summary>
        public IEnumerable<int> EmployeesId { get; set; }
        /// <summary>
        /// نوع درخواست جابه جایی
        /// </summary>
        [Display(Name = nameof(TransferRequestTypeId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public int? TransferRequestTypeId { get; set; }
        /// <summary>
        /// تیم کاری  جدید
        /// </summary>
        [Display(Name = nameof(TeamWorkId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public int? TeamWorkId { get; set; }
        /// <summary>
        /// شیفت کاری جدید
        /// </summary>
        [Display(Name = nameof(WorkGroupId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public int? WorkGroupId { get; set; }
        /// <summary>
        /// تاریخ جابه جایی
        /// </summary>
      //  [Required(ErrorMessageResourceName = "RequiredTeamTransferReturnDateAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        [Display(Name = nameof(TeamTransferReturnDate), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public DateTime? TeamTransferReturnDate { get; set; }
        /// <summary>
        /// تیم کاری
        /// </summary>
        public List<SearchTeamWorkModel> AvailableTeamWork { get; set; } = new List<SearchTeamWorkModel>();
        /// <summary>
        /// شیفت کاری
        /// </summary>
        public List<SearchWorkGroupModel> AvailableWorkGroup { get; set; } = new List<SearchWorkGroupModel>();
        public string InsertUser { get; set; }
        public string DomainName { get; set; }

        /// <summary>
        /// تاریخ جابه جایی
        /// </summary>
       // [Required(ErrorMessageResourceName = "RequiredTeamTransferReturnDateAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        [Display(Name = nameof(ShiftTransferReturnDate), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public DateTime? ShiftTransferReturnDate { get; set; }
    }
}
