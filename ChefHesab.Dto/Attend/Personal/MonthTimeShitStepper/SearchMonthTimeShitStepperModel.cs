using Ksc.HR.Share.General;
using Ksc.HR.Resources.Transfer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSC.Common.Filters.Models;

namespace Ksc.HR.DTO.Personal.MonthTimeShitStepper
{
    public class SearchMonthTimeShitStepperModel : FilterRequest
    {
        public int Id { get; set; } // Id (Primary key)
        public string Label { get; set; } // Label
        public int OrderNo { get; set; } // OrderNo
        public string Action { get; set; } // Action
        public bool IsActive { get; set; } // IsActive
        public string ProccessName { get; set; }
        public int YearMonth { get; set; }
    }
}
