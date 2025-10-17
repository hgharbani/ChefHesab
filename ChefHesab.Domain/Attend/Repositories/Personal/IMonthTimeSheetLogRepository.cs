using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;

namespace Ksc.Hr.Domain.Repositories.Personal
{
    public interface IMonthTimeSheetLogRepository : IRepository<MonthTimeSheetLog, int>
    {
        bool CheckRequiredStepForMonthSheet(int yearMonth);
        IQueryable<MonthTimeSheetLog> GetMonthTimeSheetLogByMonthAsNoTracking(int yearMonth);
    }
}

