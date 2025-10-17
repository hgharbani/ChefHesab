using Ksc.HR.DTO.Personal.EmployeeWorkGroups;
using Ksc.HR.DTO.Transfer.Transfer_Request;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.Personal
{
    public interface IEmployeeWorkGroupService
    {
        Task EmployeeWorkGroupTransferManagement(ResultEmployeeTransferModel model);
        FilterResult<EmployeeWorkGroupModel> GetEmployeeWorkGroups(EmployeeWorkGroupModel model);
    }
}
