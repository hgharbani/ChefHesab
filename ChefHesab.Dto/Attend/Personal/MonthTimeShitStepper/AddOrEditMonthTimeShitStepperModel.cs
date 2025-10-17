using Ksc.HR.Resources.Personal;
using Ksc.HR.Share.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.MonthTimeShitStepper
{
    public class AddOrEditMonthTimeShitStepperModel
    {
        public int Id { get; set; } // Id (Primary key)
        [Display(Name = nameof(Label), ResourceType = typeof(MonthTimeShitStepperResource))]
        [Required(ErrorMessageResourceName = "RequiredTitleAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Transfer.Request))]
        public string Label { get; set; } // Label
        [Display(Name = nameof(OrderNo), ResourceType = typeof(MonthTimeShitStepperResource))]
        [Required(ErrorMessageResourceName = "RequiredTitleAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Transfer.Request))]
        public int OrderNo { get; set; } // OrderNo
        [Display(Name = nameof(Action), ResourceType = typeof(MonthTimeShitStepperResource))]
        [Required(ErrorMessageResourceName = "RequiredTitleAttributeErrorMessage", ErrorMessageResourceType = typeof(Ksc.HR.Resources.Transfer.Request))]
        public string Action { get; set; } // Action
        [Display(Name = nameof(IsActive), ResourceType = typeof(MonthTimeShitStepperResource))]
        public bool IsActive { get; set; } // IsActive
        [Display(Name = nameof(IsShowDetails), ResourceType = typeof(MonthTimeShitStepperResource))]
        public bool IsShowDetails { get; set; }

        [Display(Name = nameof(IsRequiredForMonthSheet), ResourceType = typeof(MonthTimeShitStepperResource))]
        public bool IsRequiredForMonthSheet { get; set; }
        [Display(Name = nameof(FunctionDetails), ResourceType = typeof(MonthTimeShitStepperResource))]
        public string FunctionDetails { get; set; } // FunctionDetails
    }
}
