//using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItems;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;
using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using Ksc.HR.DTO.Personal.EmployeeLongTermAbsences;
using KSC.Common;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.Personal
{
    public interface IEmployeeAttendAbsenceManagementMiddleService1
    {
        KscResult AddUpdateEmplyeeAttendAbsenseItems(List<AddEmployeeLongTermAbsencesModel> models);
         Task<KscResult> AddLogTermsAbsenceItemForMisApi(AddEmployeeLongTermAbsencesModel item);
    }
}
