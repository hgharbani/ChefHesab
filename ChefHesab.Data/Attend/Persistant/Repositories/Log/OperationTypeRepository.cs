using KSC.Infrastructure.Persistance;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Entities.Log;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Repositories.Log;

namespace Ksc.HR.Data.Persistant.Repositories
{
    public partial class OperationTypeRepository : EfRepository<OperationType, int>, IOperationTypeRepository
    {
        private readonly KscHrContext _kscHrContext;

        public OperationTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}

