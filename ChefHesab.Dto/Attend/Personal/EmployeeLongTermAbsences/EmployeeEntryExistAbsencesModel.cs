using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using Ksc.HR.DTO.WorkShift.ShiftConcept;
using Ksc.HR.DTO.WorkShift.ShiftConceptDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeLongTermAbsences
{
    public class EmployeeEntryExistAbsencesModel
    {
        public long WorkCalendarId { get; set; }
        public int EmployeeId { get; set; }
        public int RollCallDefinitionId { get; set; }
        public string RollCallDefinitionCode { get; set; }
        public int WorkGroupId { get; set; }
        public string WorkGroupCode { get; set; }


        public int TeamWorkId { get; set; }
        public int WorkTimeId { get; set; }
        public string WorkTimeTitle { get; set; }
        public string FullName { get; set; }
        public int PersonalNumber { get; set; }
        public string ShiftCodeTile { get; set; }
        public int ShiftCodeId { get; set; }
        public bool IsExistEmployeeAttendAbsenceItem { get; set; }
        public List<EntryExitList> EntryExitLists { get; set; }
        public string Entry1 { get; set; }
        public string Entry2 { get; set; }
        public string Entry3 { get; set; }
        public string Exist1 { get; set; }
        public string Exist2 { get; set; }
        public string Exist3 { get; set; }
        public string SelecTedDate { get; set; }
        public int ShiftConceptDetailsId { get; set; }
        public int TeamWorkCode { get; set; }
        public int? WorkCityId { get; set; }
        public SearchShiftConceptModel ShiftConceptDetail { get; set; }=new SearchShiftConceptModel();
        public SearchShiftConceptModel OldShiftConceptDetail { get; set; } = new SearchShiftConceptModel();
        public string RollCallCategoryCode { get; set; }
        public bool InActiveForAllUser { get; set; }
        public bool ActiveForOfficialUser { get; set; }
        public bool IsUserInsertData { get; set; }
        public bool IsOfficialAttendAbcense { get; set; }
        public bool ActiveForAllUser { get; set; }
        public string ColorCode { get; set; }
        public bool IsUserManagerTeam { get; set; }
        public string ShiftConceptDetailsTitle { get; set; }
        public string ShiftConceptDetailsCode { get; set; }
        public int OldShiftConceptDetailId { get; set; }
        public string OldShiftConceptDetailCode { get; set; }
        public string OldShiftConceptDetailTitle { get; set; }
        public bool UserInsertDataIsManager { get; set; }
        public bool IshaveLongTearm { get; set; }
        public bool IsValidForDeleteAbsenceItem { get; set; }
    }
}
