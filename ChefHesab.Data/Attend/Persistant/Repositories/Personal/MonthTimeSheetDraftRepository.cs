using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities.Personal;
using Ksc.HR.Data.Persistant.Context;
using Ksc.Hr.Domain.Repositories;
using Ksc.Hr.Domain.Repositories.Personal;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.Hr.Data.Persistant.Repositories.Personal
{
    public partial class MonthTimeSheetDraftRepository : EfRepository<MonthTimeSheetDraft, int>, IMonthTimeSheetDraftRepository
    {

        private readonly KscHrContext _kscHrContext;
        public MonthTimeSheetDraftRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<MonthTimeSheetDraft> GetMonthTimeSheetDraftByEmployeeIdYearMonth(int employeeId, int yearMonth)
        {
            return _kscHrContext.MonthTimeSheetDrafts.Where(x => x.EmployeeId == employeeId && x.YearMonth == yearMonth);
        }
        public IQueryable<MonthTimeSheetDraft> GetMonthTimeSheetDraftByYearMonth(int yearMonth)
        {
            return _kscHrContext.MonthTimeSheetDrafts.Where(x => x.YearMonth == yearMonth);
        }
        public int GetForcedOverTimeByEmployeeIdYearMonth(int employeeId, int yearMonth)
        {
            int forcedOverTime = 0;
            var query = GetMonthTimeSheetDraftByEmployeeIdYearMonth(employeeId, yearMonth).ToList();
            if (query.Count() != 0)
                forcedOverTime = query.Sum(x => x.ForcedOverTime);
            return forcedOverTime;
        }


        public IQueryable<MonthTimeSheetDraft> GetMonthTimeSheetDraftByEmployeeIdYearMonth_Team(List<int> employeeIds, int yearMonth)
        {
            return _kscHrContext.MonthTimeSheetDrafts.Where(x => employeeIds.Contains(x.EmployeeId) && x.YearMonth == yearMonth);
        }
        public int GetForcedOverTimeByEmployeeIdYearMonth_Team(List<int> employeeIds, int yearMonth)
        {
            int forcedOverTime = 0;
            var query = GetMonthTimeSheetDraftByEmployeeIdYearMonth_Team(employeeIds, yearMonth).ToList();
            if (query.Count() != 0)
                forcedOverTime = query.Sum(x => x.ForcedOverTime);
            return forcedOverTime;
        }
    }
}

