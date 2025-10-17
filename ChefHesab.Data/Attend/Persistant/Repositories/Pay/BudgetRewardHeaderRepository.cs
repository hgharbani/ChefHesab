using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.HR.Share.Model.Pay;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Pay;

public class BudgetRewardHeaderRepository : EfRepository<BudgetRewardHeader, int>, IBudgetRewardHeaderRepository
{
    private readonly KscHrContext _context;

    public BudgetRewardHeaderRepository(KscHrContext context) : base(context)
    {
        _context = context;
    }


    public BudgetRewardHeaderResponseSharedDto GetBudgetRewardByTypeAndSalaryDate(int salaryDate, int budgetTypeId)
    {
        var data = _context
            .BudgetRewardHeader
            .Include(x => x.BudgetRewardType)
            .ThenInclude(x => x.AccountCode)
            .Include(x => x.BudgetRewardDetails)
            .ThenInclude(x => x.Chart_JobPosition)
            .ThenInclude(x => x.Chart_Structure)
            .Where(x => x.BudgetRewardTypeId == budgetTypeId)
            .Where(x => x.StartYearMonth <= salaryDate && x.EndYearMonth >= salaryDate)
            .Where(x => x.IsActive)
            .FirstOrDefault();
        if (data == null)
        {
            throw new Exception($"برای {salaryDate} بودجه یافت نشد");
        }


        var result = new BudgetRewardHeaderResponseSharedDto()
        {
            Id = data.Id,
            BudgetRewardTypeId = data.BudgetRewardTypeId,
            BudgetRewardTypeTitle = data.BudgetRewardType.Title,
            StartYearMonth = data.StartYearMonth,
            EndYearMonth = data.EndYearMonth,
            MinRewardPerPerson = data.MinRewardPerPerson,
            MaxRewardPerPerson = data.MaxRewardPerPerson,
            IsConfirmed = data.IsConfirmed,
            IsActive = data.IsActive,
            InsertUser = data.InsertUser,
            Details = data.BudgetRewardDetails.Select(x => new BudgetRewardDetailResponseSharedDto
            {
                Id = x.Id,
                BudgetRewardHeaderId = x.BudgetRewardHeaderId,
                BudgetAmount = x.BudgetAmount,
                ContractBudgetAmount = x.ContractBudgetAmount,
                JobPositionId = x.JobPositionId,
                JobPositionTitle = x.Chart_JobPosition.Title,
                StructureId = x.Chart_JobPosition.Chart_Structure.Id,
                StructureTitle = x.Chart_JobPosition.Chart_Structure.Title,
                RemainingBudgetAmount = x.RemainingBudgetAmount,
                SpentBudget = x.SpentBudget,
                IsNeededAssistance = x.IsNeededAssistance,
                IsActive = x.IsActive
            }).ToList()
        };

        return result;
    }


}
