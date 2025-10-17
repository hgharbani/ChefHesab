using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardCategoryRepository : EfRepository<RewardCategory, int>, IRewardCategoryRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardCategoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}

