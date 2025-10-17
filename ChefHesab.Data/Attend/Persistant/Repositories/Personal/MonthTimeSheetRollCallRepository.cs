using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities.Personal;
using Ksc.HR.Data.Persistant.Context;
using Ksc.Hr.Domain.Repositories;
using Ksc.Hr.Domain.Repositories.Personal;
using Ksc.HR.Domain.Entities.Personal;
using Microsoft.EntityFrameworkCore;

namespace Ksc.Hr.Data.Persistant.Repositories.Personal
{
    public partial class MonthTimeSheetRollCallRepository : EfRepository<MonthTimeSheetRollCall, long>, IMonthTimeSheetRollCallRepository
    {

        private readonly KscHrContext _kscHrContext;
        public MonthTimeSheetRollCallRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<MonthTimeSheetRollCall> GetMonthTimeSheetRollCall()
        {
            return _kscHrContext.MonthTimeSheetRollCalls.AsQueryable().Include(m => m.RollCallDefinition).ThenInclude(m => m.RollCallSalaryCodes).AsQueryable();
        }
        public IQueryable<MonthTimeSheetRollCall> GetAllMonthTimeSheetRollCall()
        {
            return _kscHrContext.MonthTimeSheetRollCalls;
        }
    }
}

