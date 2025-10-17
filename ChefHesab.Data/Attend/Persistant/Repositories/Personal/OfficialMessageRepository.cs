using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities.Personal;
using Ksc.HR.Data.Persistant.Context;
using Ksc.Hr.Domain.Repositories;
using Ksc.Hr.Domain.Repositories.Personal;

namespace Ksc.Hr.Data.Persistant.Repositories.Personal
{
    public partial class OfficialMessageRepository : EfRepository<OfficialMessage, int>, IOfficialMessageRepository
    {

        private readonly KscHrContext _kscHrContext;
        public OfficialMessageRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}

