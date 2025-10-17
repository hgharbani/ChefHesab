using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.Pay;

public class BudgetRewardDetailRepository : EfRepository<BudgetRewardDetail, int>, IBudgetRewardDetailRepository
{
    private readonly KscHrContext _context;

    public BudgetRewardDetailRepository(KscHrContext context) : base(context)
    {
        _context = context;
    }

 
}
