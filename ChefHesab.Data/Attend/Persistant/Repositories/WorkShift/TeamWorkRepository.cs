using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class TeamWorkRepository : EfRepository<TeamWork, int>, ITeamWorkRepository
    {
        private readonly KscHrContext _kscHrContext;
        public TeamWorkRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<TeamWork> GetTeamWorksByWinUser(string winUser)
        {

            var teamworksBywinUser = _kscHrContext.ViewMisEmployeeSecurities.Where(a => a.WindowsUser.ToLower() == winUser).AsNoTracking();
            var teamCodes = teamworksBywinUser.Select(a => a.TeamCode.ToString()).ToList();
            var query = _kscHrContext.TeamWorks.Where(a => a.IsActive && teamCodes.Contains(a.Code)).AsQueryable();

            return query;
        }

        public TeamWork GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;
            var entity = _kscHrContext.TeamWorks.AsQueryable().Include(x => x.OverTimeDefinition).FirstOrDefault(a => a.Code == code);
            return entity;
        }
        public IQueryable<TeamWork> GetAllQueryable()
        {

            return _kscHrContext.TeamWorks;
        }
        public TeamWork GetTeamWorkByIdIncludedTeamWorkCategory(int id)
        {
            var entity = _kscHrContext.TeamWorks.Include(x => x.TeamWorkCategory).FirstOrDefault(a => a.Id == id);
            return entity;
        }

       
    }
}
