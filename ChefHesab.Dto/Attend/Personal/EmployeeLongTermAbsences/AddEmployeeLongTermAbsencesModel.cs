using Ksc.HR.DTO.OnCall.Employee;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeLongTermAbsences
{
    public class AddEmployeeLongTermAbsencesModel
    {
        public long Id { get; set; }
        public int EmployeeId { get; set; } // EmployeeId
        public int EmployeeTypeId { get; set; } // EmployeeId
        public int RollCallCategoryId { get; set; } // EmployeeId

        /// <summary>
        /// تاریخ شروع
        /// </summary>
        [DisplayName("تاریخ شروع")]
        public DateTime? AbsenceStartDate { get; set; }
        public string ShamsiAbsenceStartDate { get; set; }

        /// <summary>
        /// تاریخ پایان
        /// </summary>
        [DisplayName("تاریخ پایان")]
        public DateTime? AbsenceEndDate { get; set; }
        public string ShamsiAbsenceEndDate { get; set; }

        /// <summary>
        /// تعدا روزهای غیبت
        /// </summary>
        [DisplayName("تعداد روزهای غیبت")] 
        public int AbsenceDayCount { get; set; }

        public DateTime? EmployeeRegisterDate { get; set; }

        public string PersonalNumbers { get; set; }

        /// <summary>
        /// کد حضور غیاب
        /// </summary>
        [DisplayName("کد حضور غیاب")] 
        public int RollCallDefinitionId { get; set; }
        //public bool LongTermAbsenceCheck { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        // Foreign keys

        /// <summary>
        /// Parent Employee pointed by [EmployeeLongTermAbsence].([EmployeeId]) (FK_EmployeeLongTermAbsence_Employee)
        /// </summary>
        public List<SearchEmployeeModel> AvilableEmployees { get; set; } // FK_EmployeeLongTermAbsence_Employee

        /// <summary>
        /// Parent RollCallDefinition pointed by [EmployeeLongTermAbsence].([RollCallDefinitionId]) (FK_EmployeeLongTermAbsence_RollCallDefinition)
        /// </summary>
        public List<SearchRollCallDefinicationModel> AvilableRollCallDefinition { get; set; } // FK_EmployeeLongTermAbsence_RollCallDefinition
        public string CurrentUserName { get; set; }
        public int WorkTimeId { get; set; }
        public int WorkCalendarId { get; set; }
        public bool LongTermAbsenceCheck { get; set; }
    }
}
