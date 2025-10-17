using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class V_JobPositionRepository : EfRepository<V_JobPosition>, IV_JobPositionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public V_JobPositionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;

        }
    }
}

