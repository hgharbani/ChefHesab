using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class ConfirmInterdictStatusRepository : EfRepository<ConfirmInterdictStatus, int>, IConfirmInterdictStatusRepository
    {
        private readonly KscHrContext _kscHrContext;

        public ConfirmInterdictStatusRepository(KscHrContext context) : base(context)
        {
            _kscHrContext = context;
        }
    }
}

