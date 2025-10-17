using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;

using Ksc.HR.Domain.Entities.Security;
using Ksc.HR.Domain.Repositories.Security;
using Ksc.HR.DTO.Security;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Domain.Entities.Reward;

namespace Ksc.HR.Data.Persistant.Repositories.Security
{
    public class UserDefinitionRepository : EfRepository<UserDefinition, int>, IUserDefinitionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public UserDefinitionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<UserDefinition> GetUserDefinition_ByRelated()
        {
            var query = _kscHrContext.UserDefinitions
                .Include(x => x.Employee)


               .Include(x => x.UserDefinitionSecurityTeamWorks)
                .ThenInclude(x => x.UserDefinitionSecurityPriorityStatus)

                .Include(x => x.UserDefinitionSecurityTeamWorks)
                .ThenInclude(x => x.TeamWork)
               ;
            return query;
        }

        public IQueryable<UserDefinition> GetUserDefinitionByRelated()
        {
            var query = _kscHrContext.UserDefinitions

               .Include(x => x.Employee)
               .Include(x => x.UserDefinitionSecurityTeamWorks)

                .AsNoTracking();
            return query;
        }
    }
}
