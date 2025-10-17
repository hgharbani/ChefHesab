using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Rule;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class ConfirmInterdictMessageRepository : EfRepository<ConfirmInterdictMessage, int>, IConfirmInterdictMessageRepository
    {
        private readonly KscHrContext _kscHrContext;

        public ConfirmInterdictMessageRepository(KscHrContext context) : base(context)
        {
            _kscHrContext = context;
        }

      
    }
}

