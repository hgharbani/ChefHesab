using Ksc.HR.Data.Persistant.Context;
using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities.Personal;
using Ksc.Hr.Domain.Repositories;
using Ksc.Hr.Domain.Repositories.Personal;
using Ksc.HR.Domain.Entities.Personal;
using Microsoft.EntityFrameworkCore;
namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public partial class MonthTimeSheetLogRepository : EfRepository<MonthTimeSheetLog, int>, IMonthTimeSheetLogRepository
    {

        private readonly KscHrContext _kscHrContext;
        public MonthTimeSheetLogRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<MonthTimeSheetLog> GetMonthTimeSheetLogByMonthAsNoTracking(int yearMonth)
        {
            return _kscHrContext.MonthTimeSheetLogs.Where(x => x.YearMonth == yearMonth).Include(x => x.MonthTimeShitStepper).AsNoTracking();
        }
        public bool CheckRequiredStepForMonthSheet(int yearMonth)
        {

            var requiredSteps = _kscHrContext.MonthTimeShitSteppers.Where(x => x.IsRequiredForMonthSheet && x.IsActive).Select(x => x.Id).ToList();
            var logStepper= _kscHrContext.MonthTimeSheetLogs.Count(x => x.YearMonth == yearMonth && requiredSteps.Contains(x.MonthTimeShitStepperId) );
            return (requiredSteps.Count() == logStepper);
        }
    }
}
