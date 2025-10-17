using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;

using Ksc.HR.Domain.Entities.Security;
using Ksc.HR.Domain.Repositories.Security;

namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class UserDefinitionSecurityPriorityStatusRepository : EfRepository<UserDefinitionSecurityPriorityStatus, int>, IUserDefinitionSecurityPriorityStatusRepository
    {
        private readonly KscHrContext _kscHrContext;
        public UserDefinitionSecurityPriorityStatusRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }



    }
}

