using KSC.Domain;
using Ksc.HR.Domain.Entities.Pay;


namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IBudgetRewardEmployeeHistoryRepository : IRepository<BudgetRewardEmployeeHistory, int>
    {

        IQueryable<BudgetRewardEmployeeHistory> GetBudgetRewardEmployeeHistoryListById(int budgetRewardEmployeeId);

        IQueryable<BudgetRewardEmployeeHistory> GetBudgetRewardEmployeeHistoryListByEmployeeId(int employeeId);
    }
}
