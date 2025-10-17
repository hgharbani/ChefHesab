using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Security;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Security
{
    public interface IUserDefinitionSecurityTeamWorkRepository : IRepository<UserDefinitionSecurityTeamWork, int>
    {
        IQueryable<UserDefinitionSecurityTeamWork> GetUserDefinitionSecurityTeamWorkByRelated();
    }
}
