using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class AnalysisAttenAbcenseResultModel
    {
        public List<EmployeeAttendAbsenceAnalysisModel> EmployeeAttendAbsenceAnalysisModel { get; set; }
        public bool HasItem { get; set; }
        public bool IsValidUnVaccine { get; set; }
        public DateTime? UnVaccineValidDate { get; set; }
        public int? VaccineDosage { get; set; }
        public TimeSettingDataModel TimeSettingDataModel { get; set; }
        public List<int> RollCallDefinitionIdForVaccinationCheck { get; set; }
        public bool InvalidForcedOvertime { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public double? AttendTimeInTemprorayTime { get; set; }  
    }


    public class EmployeeDontHaveExist
    {
        public string EmployeeNumber { get; set; }
        public string FullName { get; set; }
        public string TeamWorkCode { get; set; }
        public string EntrayDate { get; set; }
        public string ExistDate { get; set; }
        public bool IsExistDateOncall { get; set; }
        public bool IsEntrayDateOncall { get; set; }
    }
}
