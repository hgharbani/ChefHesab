using Ksc.HR.Resources.Personal;
using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using KSC.Common.Filters.Models;
using Ksc.HR.DTO.WorkShift.ShiftConcept;
using System;
using System.Collections.Generic;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class MISEmployeeModel : FilterRequest
    {
        public int Id { get; set; } // Id (Primary key)

        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)
        public int EmployeeId { get; set; } // Id (Primary key)
        public int? JobPositionId { get; set; } // JobPositionId

        /// <summary>
        /// تاریخ شروع پست سازمانی 
        /// </summary>
        public DateTime? JobPositionStartDate { get; set; }//JobPositionStartDate


    }

}
