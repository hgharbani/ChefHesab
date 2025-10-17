using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories.ODSViews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Entities.Reward;

namespace Ksc.HR.Data.Persistant.Repositories.ODSViews
{
    public class ViewMisEmployeeSecurityRepository : EfRepository<ViewMisEmployeeSecurity>, IViewMisEmployeeSecurityRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewMisEmployeeSecurityRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<ViewMisEmployeeSecurity> GetTeamByWindowsUser(string windowsUser)
        {
            var model = _kscHrContext.ViewMisEmployeeSecurities.Where(x => x.WindowsUser.Trim() == windowsUser.Trim());
            return model;
        }

        public IQueryable<ViewMisEmployeeSecurity> GetAllQueryable()
        {
            var model = _kscHrContext.ViewMisEmployeeSecurities;
            return model;
        }
        public async Task<ViewMisEmployeeSecurity> GetEmployeesSecurityByWinUser(string userName)
        {

            return await _kscHrContext.ViewMisEmployeeSecurities.OrderBy(x=>x.WindowsUser).Where(x => x.WindowsUser.Trim() == userName.Trim().ToUpper()).FirstOrDefaultAsync();
          
        }
        public IQueryable<ViewMisEmployeeSecurity> GetAllData()
        {
            return _kscHrContext.ViewMisEmployeeSecurities.Where(x => x.DisplaySecurity == 1);
        }

        public IQueryable<ViewMisEmployeeSecurity> GetEmployeeSecurityIsActive()
        {
            return  _kscHrContext.ViewMisEmployeeSecurities.Where(x => x.TeamWorkIsActive == true).AsNoTracking();



        }
      
    }
}
