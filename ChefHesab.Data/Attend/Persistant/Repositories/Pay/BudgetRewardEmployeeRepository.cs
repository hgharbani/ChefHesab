using EFCore.BulkExtensions;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.HR.Share.Model.BudgetRewardStatus;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Share.Extention;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class BudgetRewardEmployeeRepository : EfRepository<BudgetRewardEmployee, int>, IBudgetRewardEmployeeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public BudgetRewardEmployeeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public void UpdateRange(List<BudgetRewardEmployee> list)
        {
            _kscHrContext.UpdateRange(list);
        }



        public BudgetRewardEmployee GetOne(int id)
        {
            return _kscHrContext.BudgetRewardEmployee.AsQueryable().First(a => a.Id == id);
        }

        public IQueryable<BudgetRewardEmployee> BudgetRewardEmployeeByRelated()
        {
            var query = _kscHrContext.BudgetRewardEmployee
                .Include(x => x.BudgetRewardEmployeeHistories)
                .Include(x => x.BudgetRewardStatus)
                .Include(x => x.Employee)
                .AsQueryable().AsNoTracking();
            return query;

        }

        public bool DeleteBudgetRewardEmployeesById(int id, int statusId)
        {
            var oneBudget = _kscHrContext.BudgetRewardEmployee
                .Include(x => x.BudgetRewardEmployeeHistories)
                .FirstOrDefault(a => a.Id == id);
            if (oneBudget != null)
            {

                if (oneBudget.BudgetRewardStatusId != statusId)
                {
                    return false;
                }
                //اگر فقط یک تاریخچه دارد
                //if (oneBudget.BudgetRewardEmployeeHistories.Count() != 1)
                //    return false;
                //


                _kscHrContext.BudgetRewardEmployeeHistories.RemoveRange(oneBudget.BudgetRewardEmployeeHistories);
                _kscHrContext.BudgetRewardEmployee.Remove(oneBudget);

                return true;
            }
            else
            {
                return false;
            }

        }


        public IQueryable<BudgetRewardEmployee> GetConfirmedBudgetRewardEmployee(int SalaryDate, bool isAsNoTracking)
        {
            var query = _kscHrContext
            .BudgetRewardEmployee
            .Where(x => x.BudgetRewardStatusId == EnumBudgetRewardStatus.Confirmed.Id
            && x.YearMonth == SalaryDate)
            .Include(x => x.BudgetRewardDetail)
            .ThenInclude(x => x.BudgetRewardHeader)
             .Include(x => x.BudgetRewardDetail)
            .ThenInclude(x => x.Chart_JobPosition)
            .Include(x => x.Employee)
            .ThenInclude(x => x.TeamWork)
            .AsQueryable();
            if (isAsNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }

        /// <summary>
        /// بدست اوردن لیست افراد کارانه خدمت فعال
        /// 
        /// </summary>
        /// <param name="SalaryDate"></param>
        /// <param name="StatusID"></param>
        /// <param name="isAsNoTracking"></param>
        /// <returns></returns>
        public IQueryable<BudgetRewardEmployee> GetBudgetRewardEmployeeByStatus(int SalaryDate, int? StatusID, bool isAsNoTracking)
        {
            var query = _kscHrContext
            .BudgetRewardEmployee
            .WhereIf(SalaryDate > 0, x => x.YearMonth == SalaryDate)
            .WhereIf(StatusID.HasValue, x => x.BudgetRewardStatusId == StatusID)
            .Include(x => x.BudgetRewardDetail)
            .ThenInclude(x => x.BudgetRewardHeader)
             .Include(x => x.BudgetRewardDetail)
            .ThenInclude(x => x.Chart_JobPosition)
            .Include(x => x.Employee)
            .ThenInclude(x => x.TeamWork)
            .AsQueryable();



            if (isAsNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }

        public List<Tuple<int, long>> GetSummBudjetRewardWithSalaryDAte(int salaryDate)
        {
            var query = _kscHrContext.BudgetRewardEmployee.Where(x => x.YearMonth == salaryDate &&
            x.BudgetRewardStatusId == EnumBudgetRewardStatus.SavedDeduction.Id).GroupBy(a => a.BudgetRewardDetailId)
            .Select(a => new Tuple<int, long>(a.Key, a.Sum(x => x.RewardAmount))).ToList();
            return query;
    }
    }
}

