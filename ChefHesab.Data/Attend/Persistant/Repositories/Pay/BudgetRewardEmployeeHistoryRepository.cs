using EFCore.BulkExtensions;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;

using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class BudgetRewardEmployeeHistoryRepository : EfRepository<BudgetRewardEmployeeHistory, int>, IBudgetRewardEmployeeHistoryRepository
    {
        private readonly KscHrContext _kscHrContext;
        public BudgetRewardEmployeeHistoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<BudgetRewardEmployeeHistory> GetBudgetRewardEmployeeHistoryListById(int budgetRewardEmployeeId)
        {
            var query =  _kscHrContext.BudgetRewardEmployeeHistories.Where(a => a.BudgetRewardEmployeeId == budgetRewardEmployeeId)
                .Include(x => x.BudgetRewardEmployee)
                .ThenInclude(x => x.BudgetRewardStatus)
                .Include(x => x.Chart_JobPosition)
                .Include(x => x.Employee)
                .AsQueryable().AsNoTracking();
             
            return query;
        }
        public IQueryable<BudgetRewardEmployeeHistory> GetBudgetRewardEmployeeHistoryListByEmployeeId(int employeeId)
        {
             
            var query = _kscHrContext.BudgetRewardEmployeeHistories.Where(a => a.BudgetRewardEmployee.EmployeeId == employeeId)
                .Include(x => x.BudgetRewardEmployee)
                .ThenInclude(x => x.BudgetRewardStatus)
                .Include(x => x.Chart_JobPosition)
                .Include(x => x.Employee)
                .AsQueryable().AsNoTracking();

            return query;
        }


    }
}

