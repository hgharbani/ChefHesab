using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Share.Model.Rule;
using System.Reflection.PortableExecutable;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardBaseDetailRepository : EfRepository<RewardBaseDetail, int>, IRewardBaseDetailRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardBaseDetailRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        /// <summary>
        /// دریافت اطلاعات فعال جدول
        /// </summary>
        /// <returns></returns>
        public IQueryable<RewardBaseDetail> GetAllActive()
        {
            var result = _kscHrContext.RewardBaseDetails.Where(x => x.IsActive).AsQueryable();
            return result;
        }

        /// <summary>
        /// دریافت اطلاعات بر اساس شناسه جدول RewardBaseHeader
        /// </summary>
        /// <param name="BaseHeaderId">ضروری</param>
        /// <param name="UnitTypeId">اختیاری</param>
        /// <returns>AsNoTracking</returns>
        public IQueryable<RewardBaseDetail> GetByBaseHeaderId(int BaseHeaderId, int? UnitTypeId)
        {
            var result = GetAllActive().Include(x=>x.RewardUnitType).AsNoTracking().Where(x => x.RewardBaseHeaderId == BaseHeaderId);
            if (UnitTypeId.HasValue)
                result = result.Where(x => x.RewardUnitTypeId == UnitTypeId);
            return result;
        }


        public double GetkaraneMabnaMah(int headerId,double ToolidMoadel)
        {
            double? karaneMabnaMah = 0;

            var detailMonthly = this.GetByBaseHeaderId(headerId, EnumRewardUnitType.Monthly.Id).ToList();
            foreach (var item in detailMonthly)
            {
                if (ToolidMoadel > item.MinimumOfUnit && ToolidMoadel >= item.MaximumOfUnit)
                    karaneMabnaMah += (item.BaseAmount * (item.MaximumOfUnit - item.MinimumOfUnit));

                if (ToolidMoadel > item.MinimumOfUnit && ToolidMoadel < item.MaximumOfUnit)
                    karaneMabnaMah += (item.BaseAmount * (ToolidMoadel - item.MinimumOfUnit));

            }

            return karaneMabnaMah.Value;
        }
    }
}

