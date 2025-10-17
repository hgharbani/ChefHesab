using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class ConfirmInterdictRepository : EfRepository<ConfirmInterdict, int>, IConfirmInterdictRepository
    {
        private readonly KscHrContext _kscHrContext;

        public ConfirmInterdictRepository(KscHrContext context) : base(context)
        {
            _kscHrContext = context;
        }

    }
}

