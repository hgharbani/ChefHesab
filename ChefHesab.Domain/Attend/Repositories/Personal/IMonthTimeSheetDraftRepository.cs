using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;

namespace Ksc.Hr.Domain.Repositories.Personal
{
    public interface IMonthTimeSheetDraftRepository : IRepository<MonthTimeSheetDraft, int>
    {
        int GetForcedOverTimeByEmployeeIdYearMonth(int employeeId, int yearMonth);
        IQueryable<MonthTimeSheetDraft> GetMonthTimeSheetDraftByEmployeeIdYearMonth(int employeeId, int yearMonth);
        IQueryable<MonthTimeSheetDraft> GetMonthTimeSheetDraftByYearMonth(int yearMonth);
        IQueryable<MonthTimeSheetDraft> GetMonthTimeSheetDraftByEmployeeIdYearMonth_Team(List<int> employeeIds, int yearMonth);
        int GetForcedOverTimeByEmployeeIdYearMonth_Team(List<int> employeeIds, int yearMonth);
    }
}

