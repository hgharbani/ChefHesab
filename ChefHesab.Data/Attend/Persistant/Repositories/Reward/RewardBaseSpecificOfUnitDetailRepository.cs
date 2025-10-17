using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Microsoft.EntityFrameworkCore;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardBaseSpecificOfUnitDetailRepository : EfRepository<RewardBaseSpecificOfUnitDetail, int>, IRewardBaseSpecificOfUnitDetailRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardBaseSpecificOfUnitDetailRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

      

    }
}

