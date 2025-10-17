using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.RollCallSalaryCode
{
    public class RollCallSalaryCodeModel
    {
        public int Id { get; set; } // Id (Primary key)
        public int RollCallDefinitionId { get; set; } // RollCallDefinitionId

        /// <summary>
        /// کد محاسبه در حقوق
        /// </summary>
        public decimal SalaryAccountCode { get; set; } // SalaryAccountCode
        public decimal EmploymentTypeCode { get; set; } // EmploymentTypeCode
        public string EmploymentTypeTitle { get; set; }
        public string SalaryAccountTitle { get; set; }
    }
}
