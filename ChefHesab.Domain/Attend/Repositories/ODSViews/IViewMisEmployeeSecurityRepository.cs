using KSC.Domain;
using Ksc.HR.Domain.Entities.ODSViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.ODSViews
{
    public interface IViewMisEmployeeSecurityRepository : IRepository<ViewMisEmployeeSecurity>
    {
        IQueryable<ViewMisEmployeeSecurity> GetAllData();
        IQueryable<ViewMisEmployeeSecurity> GetAllQueryable();
        Task<ViewMisEmployeeSecurity> GetEmployeesSecurityByWinUser(string userName);

        //public IEnumerable<ViewMisEmployeeSecurity> GetAll1();
        IQueryable<ViewMisEmployeeSecurity> GetTeamByWindowsUser(string windowsUser);
        IQueryable<ViewMisEmployeeSecurity> GetEmployeeSecurityIsActive();

    }
}
