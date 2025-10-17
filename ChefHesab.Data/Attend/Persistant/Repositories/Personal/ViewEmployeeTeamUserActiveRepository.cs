using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Personal;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class ViewEmployeeTeamUserActiveRepository : EfRepository<ViewEmployeeTeamUserActive>, IViewEmployeeTeamUserActiveRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewEmployeeTeamUserActiveRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<ViewEmployeeTeamUserActive> GetAllQueryable()
        {
            var result = _kscHrContext.ViewEmployeeTeamUserActive.AsQueryable();
            return result;
        }
        public IQueryable<ViewEmployeeTeamUserActive> GetAllAsNoTracking()
        {
            var result = GetAllQueryable().AsNoTracking();
            return result;
        }

        public IQueryable<ViewEmployeeTeamUserActive> GetByUser(string currentUser)
        {
            var query = GetAllAsNoTracking().Where(x => (x.DisplaySecurity == 1 || x.DisplaySecurity == 2) && x.WindowsUser.ToLower() == currentUser);

            return query;

        }
    }
}
