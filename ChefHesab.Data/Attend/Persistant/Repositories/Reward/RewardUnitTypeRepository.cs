using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Microsoft.EntityFrameworkCore;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardUnitTypeRepository : EfRepository<RewardUnitType, int>, IRewardUnitTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardUnitTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<RewardUnitType> GetRewardUnitTypes(bool asNoTracking = false)
        {
            var result = _kscHrContext.RewardUnitTypes.AsQueryable();
            if (asNoTracking)
            {
                result = result.AsNoTracking();
            }
            return result.AsQueryable();
        }

        public IQueryable<RewardUnitType> GetUnitTypesByCategoryId(int? categoryId, bool asNoTracking = false)
        {
            var result = GetRewardUnitTypes(asNoTracking);
            if (categoryId != null)
            {
                result = result.Where(x => x.RewardCategoryId == categoryId);
            }
            return result;
        }
    }
}

