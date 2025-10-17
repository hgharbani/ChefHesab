using Ksc.HR.Resources.Personal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.Employee
{
    /// <summary>
    /// فرجه شیر دهی
    /// </summary>
    public class EmployeeBreastfeddingModel
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        [Display(Name = nameof(BreastfeddingStartDate), ResourceType = typeof(EmployeeResource))]
        public DateTime? BreastfeddingStartDate { get; set; }
        [Display(Name = nameof(BreastfeddingEndDate), ResourceType = typeof(EmployeeResource))]
        [Required(ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public DateTime? BreastfeddingEndDate { get; set; }
        [Display(Name = nameof(IsBreastfeddingInStartShift), ResourceType = typeof(EmployeeResource))]

        public bool IsBreastfeddingInStartShift { get; set; }
        [NotMapped]
        public int? Gender { get; set; }
        [NotMapped]
        public int? MaritalStatusId { get; set; }
        public string CurrentUser { get; set; }
    }
}
