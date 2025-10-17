using Ksc.HR.Share.General;
using Ksc.HR.Resources.Transfer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.MonthTimeShitStepper
{
    public class MonthTimeShitStepperViewModel
    {
        public int Id { get; set; } // Id (Primary key)
        public string Label { get; set; } // Label
        public int OrderNo { get; set; } // OrderNo
        public string Action { get; set; } // Action
        public bool IsActive { get; set; } // IsActive
        public bool IsShowDetails { get; set; } // IsShowDetails
        public string FunctionDetails { get; set; } // FunctionDetails
        public bool IsRequiredForMonthSheet { get; set; } // IsRequiredForMonthSheet
    }
}
