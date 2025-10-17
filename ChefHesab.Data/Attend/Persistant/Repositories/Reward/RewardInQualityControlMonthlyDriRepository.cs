using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Ksc.HR.DTO.Reward.RewardInQualityControlMonthlyDri;
using Ksc.HR.Share.Model.Reward;
using KSC.Common.Filters.Models;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardInQualityControlMonthlyDriRepository : EfRepository<RewardInQualityControlMonthlyDri, int>, IRewardInQualityControlMonthlyDriRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardInQualityControlMonthlyDriRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<RewardInQualityControlMonthlyDri> GetRewardInQualityControlMonthlyDriById(int productionEfficiencyId)
        {
            var query = _kscHrContext.RewardInQualityControlMonthlyDris.Include(x => x.RewardIn)
                .Where(x => x.ProductionEfficiencyId == productionEfficiencyId)
                .AsNoTracking();

            return query;
        }

        public IQueryable<RewardInQualityControlMonthlyDri> GetRewardInQualityControlMonthlyDriByYearMonth(int yearMonth)
        {
            var query = _kscHrContext.RewardInQualityControlMonthlyDris.Include(x => x.RewardIn)
                .Where(x => x.RewardIn.YearMonth == yearMonth)
                .AsNoTracking();

            return query;
        }
        public double SumQualityControlMonthlyDriByYearMonth(int yearMonth)
        {

            double result = 0;
            try
            {
                var RewardIn = _kscHrContext.RewardIns.Where(x => x.YearMonth == yearMonth).FirstOrDefault();
                if (RewardIn != null)
                {
                    var query = _kscHrContext.RewardInQualityControlMonthlyDris.Include(x => x.RewardIn)
                    .Where(x => x.RewardIn.YearMonth == yearMonth && x.ProductionEfficiencyId == EnumProductionEfficiency.zamzam.Id)//id=5
                        .Select(x => new
                        {
                            CSuggestionFactorDriItem = x.CSuggestionFactorDri,
                        });
                    var SumMonthlyDriByYearMonth = query.Select(x => x.CSuggestionFactorDriItem).Sum() ?? 0;

                    result = (double)SumMonthlyDriByYearMonth / 2;
                }
            }
            catch (Exception ex)
            {
                return result;

            }

            return result;

        }

        public double SumQualityControlMonthlyDriByYearMonthForSpecific(int yearMonth)
        {

            double result = 0;
            try
            {
                var RewardIn = _kscHrContext.RewardIns.Where(x => x.YearMonth == yearMonth).FirstOrDefault();
                if (RewardIn != null)
                {
                    var query = _kscHrContext.RewardInQualityControlMonthlyDris.Include(x => x.RewardIn)
                    .Where(x => x.RewardIn.YearMonth == yearMonth && x.ProductionEfficiencyId == EnumProductionEfficiency.zamzam.Id)//id=5
                        .Select(x => new
                        {
                            CSuggestionFactorDriItem = x.CSuggestionFactorDri,
                        });
                    var SumMonthlyDriByYearMonth = query.Select(x => x.CSuggestionFactorDriItem).Sum() ?? 0;

                    result = (double)SumMonthlyDriByYearMonth / 2;
                }
            }
            catch (Exception ex)
            {
                return result;

            }

            return result;

        }
        public double SumQualityControlMonthlyDriSpecificByYearMonth(int yearMonth)
        {
            
            long result = 0;
            try
            {
                var RewardIn = _kscHrContext.RewardIns.Where(x => x.YearMonth == yearMonth).FirstOrDefault();
                if (RewardIn != null)
                {
                    var query = _kscHrContext.RewardInQualityControlMonthlyDris.Include(x => x.RewardIn)
                    .Where(x => x.RewardIn.YearMonth == yearMonth 
                    //&& x.ProductionEfficiencyId == EnumProductionEfficiency.zamzam.Id
                    )//id=5
                        .Select(x => new BasisDriDto() {
                            
                            MetalPercentItem = x.MetalPercent,
                            QualityPercentItem = x.QualityPercent,
                           
                            //PCN_CHNG_EHYA = METAL_MOADEL_EHYA- METAL_MOADEL_EHYA_MABNA
                        });
                    MasterModel masterModel = new MasterModel()
                    {
                        BasisDris = query.ToList()
                    };


                    return masterModel.TotalPCN;// (long)SumMonthlyDriByYearMonth / 2;
                }
            }
            catch (Exception ex)
            {
                return result;

            }

            return result;

        }


    }
}

