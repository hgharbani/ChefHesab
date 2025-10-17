using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities.Personal;
using Ksc.HR.Data.Persistant.Context;
using Ksc.Hr.Domain.Repositories;
using Ksc.Hr.Domain.Repositories.Personal;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.Hr.Data.Persistant.Repositories.Personal
{
    public partial class MonthTimeSheetIncludedRepository : EfRepository<MonthTimeSheetIncluded, long>, IMonthTimeSheetIncludedRepository
    {

        private readonly KscHrContext _kscHrContext;
        public MonthTimeSheetIncludedRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}

