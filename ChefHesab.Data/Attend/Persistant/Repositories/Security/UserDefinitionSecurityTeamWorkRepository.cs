using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;

using Ksc.HR.Domain.Entities.Security;
using Ksc.HR.Domain.Repositories.Security;
using Ksc.HR.DTO.Security;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Domain.Entities.Reward;

namespace Ksc.HR.Data.Persistant.Repositories.Security
{
    public class UserDefinitionSecurityTeamWorkRepository : EfRepository<UserDefinitionSecurityTeamWork, int>, IUserDefinitionSecurityTeamWorkRepository
    {
        private readonly KscHrContext _kscHrContext;
        public UserDefinitionSecurityTeamWorkRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<UserDefinitionSecurityTeamWork> GetUserDefinitionSecurityTeamWorkByRelated()
        {
            var query = _kscHrContext.UserDefinitionSecurityTeamWorks

               .Include(x => x.UserDefinition)
                .ThenInclude(x => x.UserDefinitionSecurityTeamWorks)

                .Include(x => x.UserDefinitionSecurityPriorityStatus)
                .Include(x => x.TeamWork);
                //.AsNoTracking();
            return query;
        }

        //public IQueryable<UserDefinitionSecurityTeamWork> GetUserDefinitionSecurityTeamWorkByRelated()
        //{
        //    var query = _kscHrContext.UserDefinitionSecurityTeamWorks

        //       .Include(x => x.Employee)

        //        .AsNoTracking();
        //    return query;
        //}
    }
}
