using KSC.Common;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.EmployeeLongTermAbsences;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.Personal
{
    public interface IEmployeeLongTermAbsencesService
    {
        bool Exists(DateTime from, DateTime to, int employeId);

        bool Exists(int id, DateTime from, DateTime to, int employeId);
        List<EmployeeLongTermAbsencesModel> GetEmployeeLongTermAbsences();
        FilterResult<EmployeeLongTermAbsencesModel> GetEmployeeLongTermAbsencesByFilter(EmployeeLongTermAbsencesModel Filter);
        EmployeeLongTermAbsence GetOne(int id);
        EditEmployeeLongTermAbsencesModel GetForEdit(int id);
        Task<KscResult> AddEmployeeLongTermAbsences(AddEmployeeLongTermAbsencesModel model);
        Task<KscResult> UpdateEmployeeLongTermAbsences(EditEmployeeLongTermAbsencesModel model);
        Task<KscResult> RemoveEmployeeLongTermAbsences(EditEmployeeLongTermAbsencesModel model);
    }
}
