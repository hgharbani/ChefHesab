using Ksc.Hr.DTO.CategoryCoefficient;
using Ksc.HR.DTO.EmployeeBase.IsarStatus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ksc.HR.DTO.Personal.Employee
{
    public class EmployeeCategoryCoefficientDto
    {
        public EmployeeCategoryCoefficientDto()
        {
            AvailableCategoryCoefficientes = new List<SearchCategoryCoefficientDto>();
        }
        public int Id { get; set; }


        [Display(Name = "ضریب")]
        public int? CategoryCoefficientId { get; set; }
        public List<SearchCategoryCoefficientDto> AvailableCategoryCoefficientes { get; set; }




    }
}
