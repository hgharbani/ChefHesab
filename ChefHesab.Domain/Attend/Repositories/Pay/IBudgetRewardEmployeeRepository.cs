using KSC.Domain;
using Ksc.HR.Domain.Entities.Pay;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IBudgetRewardEmployeeRepository : IRepository<BudgetRewardEmployee, int>
    {
        IQueryable<BudgetRewardEmployee> BudgetRewardEmployeeByRelated();
        bool DeleteBudgetRewardEmployeesById(int id, int statusId);
        IQueryable<BudgetRewardEmployee> GetBudgetRewardEmployeeByStatus(int SalaryDate, int? StatusID, bool isAsNoTracking);
        IQueryable<BudgetRewardEmployee> GetConfirmedBudgetRewardEmployee(int SalaryDate, bool isAsNoTracking);
        BudgetRewardEmployee GetOne(int id);
        List<Tuple<int, long>> GetSummBudjetRewardWithSalaryDAte(int salaryDate);
        void UpdateRange(List<BudgetRewardEmployee> list);
    }
}
