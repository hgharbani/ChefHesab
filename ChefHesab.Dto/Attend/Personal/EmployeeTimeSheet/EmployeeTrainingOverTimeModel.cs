using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeTimeSheet
{
    public class EmployeeTrainingOverTimeModel
    {
        public int YearMonth { get; set; }

        //public List<string> EmployeeNumbers { get; set; }

        //public int TrainingOverTime { get; set; }

        public string CurrentUser { get; set; }

    }

    public class GetIndustrialMedicineClassMembersVM
    {
        public int TimeRequired { get; set; }

        public int YearMonth { get; set; }

        public string EmployeeNumber { get; set; }

    }

    public class SearchIndustrialMedicineClassMembersVM
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

    }
}
