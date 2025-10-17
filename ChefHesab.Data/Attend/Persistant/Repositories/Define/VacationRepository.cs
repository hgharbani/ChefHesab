using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class VacationRepository : EfRepository<Vacation, int>, IVacationRepository
    {
        private readonly KscHrContext _kscHrContext;
        public VacationRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}

