using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Share.Model.Pay;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Pay;

public interface IBudgetRewardHeaderRepository : IRepository<BudgetRewardHeader, int>
{
    BudgetRewardHeaderResponseSharedDto GetBudgetRewardByTypeAndSalaryDate(int salaryDate, int budgetTypeId);
}
