using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Ksc.Hr.DTO.Vacation
{
    public class VacationDto
    {
        public int Id { get; set; }

        public string Description { get; set; }
        public string Code { get; set; }


        public int YearMonth { get; set; }
        public int EmployeeId { get; set; }
    }
}

