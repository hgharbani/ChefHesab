using KSC.Common;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.Employee;
using Ksc.HR.DTO.Personal.EmployeeTeamWork;
using Ksc.HR.DTO.Transfer.Transfer_Request;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.Personal
{
    public interface IEmployeeTeamWorkService
    {
        Task EmployeeTeamWorkTransferManagement(ResultEmployeeTransferModel model);
        FilterResult<EmployeeModel> GetEmployeeTeamWorkByFilter(EmployeeModel Filter);
        FilterResult<EmployeeModel> GetEmployeeTeamWorkforConfirmTimeSheetByFilter(EmployeeModel Filter);
        /// <summary>
        /// نمایش تمامی پرسنل در جدول
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        FilterResult<EmployeeTeamModel> GetAllEmployeeTeamWorkByFilter(EmployeeTeamModel Filter);
        Task<KscResult> AddEmployeeTeamWorkManagement(AddEmployeeTeamWorkModel.EmployeeTeamWork model);
        Task<KscResult> UpdateEmployeeTeamWorkManagement(AddEmployeeTeamWorkModel.EmployeeTeamWork model);
        Task<KscResult> RemoveEmployeeTeamWorkManagement(int id);
        FilterResult<EmployeeTeamModel> GetAllByFilter(EmployeeTeamModel filter);
        EmployeeTeamModel GetActiveTeamWorkByEmployeeId(int employeeId);
    }
}


