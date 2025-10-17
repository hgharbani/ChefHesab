using Ksc.HR.DTO.OnCall.Employee;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.General;

namespace Ksc.HR.DTO.Personal.EmployeeLongTermAbsences
{
    public class EmployeeLongTermAbsencesModel: FilterRequest
    {
        public int Id { get; set; } 
        public int EmployeeId { get; set; } // EmployeeId
        public string EmployeeNumber { get; set; }
        public string FullName { get; set; }
        public string TeamCode { get; set; }

        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public DateTime? AbsenceStartDate { get; set; } 

        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public DateTime? AbsenceEndDate { get; set; } 

        /// <summary>
        /// تعدا روزهای غیبت
        /// </summary>
        public int AbsenceDayCount { get; set; }

        /// <summary>
        /// کد حضور غیاب
        /// </summary>
        public int RollCallDefinitionId { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public string RollCallDefinitionTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? InsertDate { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public string InsertUser { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateDate { get; set; } 

        /// <summary>
        /// 
        /// </summary>
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

        public int RollCallCategoryId { get; set; }
        public string InsertDateShamsi { get; set; }
        public string AbsenceStartDateShamsi
        {
            get
            {
                return AbsenceStartDate.HasValue?AbsenceStartDate.ToPersianDate():"";
            }
           
        }
        public string AbsenceEndDateShamsi { 
            get {
                return AbsenceEndDate.HasValue?AbsenceEndDate.ToPersianDate():"";
            } 
        
        }

        public int SumAbsencyDayCount { get; set; }
        public int? SumDration { get; set; }
    }

    public class GroupEmployeeLongTermAbsencesModel : FilterRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; } // EmployeeId
        public string EmployeeNumber { get; set; }
        public string FullName { get; set; }
        public string TeamCode { get; set; }

        public string MinStartDate { get; set; }
        public string MaxEndDate { get; set; }

        public int SumCountDay { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public DateTime? AbsenceStartDate { get; set; }

        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public DateTime? AbsenceEndDate { get; set; }

        /// <summary>
        /// تعدا روزهای غیبت
        /// </summary>
        public int AbsenceDayCount { get; set; }
        public string SumDuration { get; set; }
        public string SumMinData { get; set; }

        
    }
}
