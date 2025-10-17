using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Share.Model.Reward;
using Microsoft.AspNetCore.Routing.Template;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardInQualityControlMonthlyPeletRepository : EfRepository<RewardInQualityControlMonthlyPelet, int>, IRewardInQualityControlMonthlyPeletRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardInQualityControlMonthlyPeletRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        /// <summary>
        /// دریافت تمام اطلاعات جدول
        /// </summary>
        /// <returns></returns>
        public IQueryable<RewardInQualityControlMonthlyPelet> GetAllData()
        {
            var result = _kscHrContext.RewardInQualityControlMonthlyPelets.AsQueryable();
            return result;
        }

        /// <summary>
        /// دریافت اطلاعات بر اساس شناسه جدول RewardIn
        /// </summary>
        /// <param name="RewardInId"></param>
        /// <returns>AsNoTracking</returns>
        public IQueryable<RewardInQualityControlMonthlyPelet> GetByRewardInId(int RewardInId)
        {
            var result = GetAllData().AsNoTracking().Where(x => x.RewardInId == RewardInId);
            return result;

        }
        public IQueryable<RewardInQualityControlMonthlyPelet> GetRewardInQualityControlMonthlyPeletByYearMonth(int yearMonth)
        {
            var query = _kscHrContext.RewardInQualityControlMonthlyPelets.Include(x => x.RewardIn)
                .Where(x => x.RewardIn.YearMonth == yearMonth)
                .AsNoTracking();

            return query;
        }

        public QualityFactorViewModel GetQualityFactorByYearMonth(int yearMonth)
        {
            var result = new QualityFactorViewModel() { YearMonth = yearMonth };
            var RewardIn = _kscHrContext.RewardIns.AsNoTracking().Include(x => x.RewardInQualityControlMonthlyPelets)
                .FirstOrDefault(x => x.YearMonth == yearMonth);
            if (RewardIn != null)
            {
                result.YearMonth = RewardIn.YearMonth;
                result.RewardInId = RewardIn.Id;

                var RewardInQualityControlMonthlyPelets = RewardIn.RewardInQualityControlMonthlyPelets.ToList();
                foreach (var item in RewardInQualityControlMonthlyPelets)
                {
                    QualityFactorDetail temp = new QualityFactorDetail();
                    temp.PeletPlantName = item.PeletPlantName;
                    temp.ProductionEfficiencyId = item.ProductionEfficiencyId;

                    if (item.PorosPercent == 24)
                        temp.Porosity = 0;
                    else
                        temp.Porosity = ((item.PorosPercent ?? 0) - 24) * 0.0007;

                    if (item.AiPercent == 2.8)
                        temp.AI = 0;
                    else
                        temp.AI = ((item.AiPercent ?? 0) - 2.8) * (-1) * 0.013;

                    if (item.CcsPercent == 330)
                        temp.Ccs = 0;
                    else
                        temp.Ccs = ((double)((item.CcsPercent ?? 0) - 330) / 10) * 0.0013;

                    if (item.SizePercent == 93)
                        temp.Size = 0;
                    else if (item.SizePercent > 93)
                        temp.Size = ((item.SizePercent ?? 0) - 93) * 0.0004;

                    if (item.B4Percent < 0.05)
                        temp.B4 = ((item.B4Percent ?? 0) - 0.05) * 0.037;
                    else if (item.B4Percent > 0.6)
                        temp.B4 = ((item.B4Percent ?? 0) - 0.6) * (-1) * 0.037;

                    if (item.RawPeletPercent < 1)
                        temp.Raw = (((item.RawPeletPercent ?? 0) * (-1)) + 1) * 0.0056;

                    result.qualityFactorDetails.Add(temp);
                }
            }
            return result;

        }
        public IQueryable<RewardInQualityControlMonthlyPelet> GetRewardInQualityControlMonthlyPeletById(int productionEfficiencyId)
        {
            var query = _kscHrContext.RewardInQualityControlMonthlyPelets.Include(x => x.RewardIn)
                .Where(x => x.ProductionEfficiencyId == productionEfficiencyId)
                .AsNoTracking();

            return query;
        }


    }
}

