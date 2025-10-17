using Ksc.HR.DTO.Personal.EmployeeEducationTime;
using KSC.Common;
using KSC.Common.Filters.Models;
using KSC.MIS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.Personal
{
    public interface IEmployeeEducationTimeService
    {
        FilterResult<EmployeeEducationTimeModel> GetEmployeeAducationDontHaveItem(SearchEmployeeEducationModel filterDate);
        Task<ReturnData<RPC005>> SaveEducationTime(RPC005 model);
    }
}
