using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.OnCall.Employee
{
    public class EditEmployeeModel
    {

        public int Id { get; set; } // Id (Primary key)
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)
        public string Name { get; set; } // Name (length: 500)
        public string Family { get; set; } // Family (length: 500)
        public int? WorkCityCityId { get; set; }
        public int? WorkCityCityProvinceCountryId { get; set; }
        public int? WorkCityCityProvinceId { get; set; }
        public int WorkTimeId { get; set; }

    }
}
