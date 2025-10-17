using Ksc.HR.Domain.Entities.ODSViews;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.ODSViews
{
    public interface IViewMisEmployeeRepository : IRepository<ViewMisEmployee>
    {
        ViewMisEmployee GetEmployeeByPersonalNumber(string employeeNumber);
        IEnumerable<ViewMisEmployee> GetMisEmployeesByJobPositionCode(string jobPositionCode);
        Task<ViewMisEmployee> GetMisEmployeesByWinUser(string userName);

        IQueryable<ViewMisEmployee> GetEmployeeByPersonalNumbers(List<string> employeeNumbers);
        ViewMisEmployee GetEmployeeByWinUser(string userName);
        IQueryable<ViewMisEmployee> GetMisEmployeesAcvtive();
    }
}
