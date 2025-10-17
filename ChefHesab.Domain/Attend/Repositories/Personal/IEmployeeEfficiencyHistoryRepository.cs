using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;

namespace Ksc.Hr.Domain.Repositories.Personal
{
    public interface IEmployeeEfficiencyHistoryRepository : IRepository<EmployeeEfficiencyHistory, int>
    {
        IQueryable<EmployeeEfficiencyHistory> GetByEmployeeId(int id);
        IQueryable<EmployeeEfficiencyHistory> GetKendoGrid(List<int> Ids, int yearMonthDayShamsi_Prev, int yearMonth);
        IQueryable<EmployeeEfficiencyHistory> GetLatestData(int yearMonth);
    }
}

