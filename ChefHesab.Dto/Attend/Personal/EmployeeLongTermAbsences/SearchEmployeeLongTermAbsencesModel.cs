using Ksc.HR.DTO.OnCall.Employee;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeLongTermAbsences
{
    public class SearchEmployeeLongTermAbsencesModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; } // EmployeeId

        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public DateTime AbsenceStartDate { get; set; }

        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public DateTime AbsenceEndDate { get; set; }

        /// <summary>
        /// تعدا روزهای غیبت
        /// </summary>
        public int AbsenceDayCount { get; set; }

        /// <summary>
        /// کد حضور غیاب
        /// </summary>
        public int RollCallDefinitionId { get; set; }
      

        // Foreign keys

        /// <summary>
        /// Parent Employee pointed by [EmployeeLongTermAbsence].([EmployeeId]) (FK_EmployeeLongTermAbsence_Employee)
        /// </summary>
        public List<SearchEmployeeModel> AvilableEmployees { get; set; } // FK_EmployeeLongTermAbsence_Employee

        /// <summary>
        /// Parent RollCallDefinition pointed by [EmployeeLongTermAbsence].([RollCallDefinitionId]) (FK_EmployeeLongTermAbsence_RollCallDefinition)
        /// </summary>
        public List<SearchRollCallDefinicationModel> AvilableRollCallDefinition { get; set; } // FK_EmployeeLongTermAbsence_RollCallDefinition
        public string RollCallDefinitionTitle { get; set; }
    }
}
