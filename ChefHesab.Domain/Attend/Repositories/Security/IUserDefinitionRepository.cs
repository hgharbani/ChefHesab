using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Security;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Security
{
    public interface IUserDefinitionRepository : IRepository<UserDefinition, int>
    {
         IQueryable<UserDefinition> GetUserDefinitionByRelated();
        IQueryable<UserDefinition> GetUserDefinition_ByRelated();
    }
}
