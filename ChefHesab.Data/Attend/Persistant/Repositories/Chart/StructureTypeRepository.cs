using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Data.Persistant.Context;

namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class StructureTypeRepository : EfRepository<Chart_StructureType, int>, IStructureTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public StructureTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}

