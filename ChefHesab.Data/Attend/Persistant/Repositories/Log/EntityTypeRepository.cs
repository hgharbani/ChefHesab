using KSC.Infrastructure.Persistance;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Entities.Log;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Repositories.Log;

namespace Ksc.HR.Data.Persistant.Repositories
{
    public partial class EntityTypeRepository : EfRepository<EntityType, int>, IEntityTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EntityTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}

