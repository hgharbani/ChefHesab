using Ksc.HR.DTO.EmployeeBase.IsarStatus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class EmployeeSacrificeDto
    {
        public EmployeeSacrificeDto()
        {
            IsarStatuses = new List<SearchIsarStatusDto>();
        }
        public int Id { get; set; }


        [Display(Name = nameof(IsarStatusId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public int? IsarStatusId { get; set; }
        public List<SearchIsarStatusDto> IsarStatuses { get; set; }

        [Display(Name = nameof(SacrificeOptionSettingId), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        public int? SacrificeOptionSettingId { get; set; }

        
       
        
        [Display(Name = nameof(SacrificePercentage), ResourceType = typeof(Ksc.HR.Resources.Personal.EmployeeResource))]
        [Range(0, 100, ErrorMessageResourceName = "RequiredAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Messages.Shared))]
        public int SacrificePercentage { get; set; }




    }
}
