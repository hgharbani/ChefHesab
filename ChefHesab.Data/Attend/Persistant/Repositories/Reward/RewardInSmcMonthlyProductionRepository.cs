using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Share.Model.Reward;
using NetTopologySuite.Index.HPRtree;
using Ksc.HR.DTO.Chart.RewardSpecific;
using Ksc.HR.Share.Model.Rule;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardInSmcMonthlyProductionRepository : EfRepository<RewardInSmcMonthlyProduction, int>, IRewardInSmcMonthlyProductionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardInSmcMonthlyProductionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        //دیتای خام تولید ماهانه به ازای rewardin که سال/ماه دارد
        public IQueryable<RewardInSmcMonthlyProduction> GetRewardInSmcMonthlyProduction(int yearmonth)
        {
            var query = _kscHrContext.RewardInSmcMonthlyProductions
                .Include(x => x.RewardIn)
                .Include(y => y.ProductionEfficiency)   
                .Where(x => x.RewardIn.YearMonth == yearmonth)
                .AsNoTracking();

            return query;
        }

        public long SumActualWeightZamzamByYearMonth(int yearMonth)
        {

            long result = 0;
            try
            {
                var RewardIn = _kscHrContext.RewardIns.Where(x => x.YearMonth == yearMonth).FirstOrDefault();
                if (RewardIn != null)
                {
                    var query = _kscHrContext.RewardInSmcMonthlyProductions.Include(x => x.RewardIn)
                    .Where(x => x.RewardIn.YearMonth == yearMonth 
                    && x.ProductionEfficiencyId == EnumProductionEfficiency.zamzam.Id)//id=5
                        .Select(x => new
                        {
                            CSuggestionFactorDriItem = x.ActualWeight,
                        });
                    var SumMonthlyDriByYearMonth = query.Select(x => x.CSuggestionFactorDriItem).Sum() ?? 0;

                    result = SumMonthlyDriByYearMonth;
                }
            }
            catch (Exception ex)
            {
                return result;

            }

            return result;

        }
        /// <summary>
        ///  1-ActualWeightItem,2-PlanWeightItem
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public Tuple<long,long> SumActualWeightAndPlanZamzam(int yearMonth)
        {

            var items = new Tuple<long, long>(0, 0);
            try
            {
                var RewardIn = _kscHrContext.RewardIns.Where(x => x.YearMonth == yearMonth).FirstOrDefault();
                if (RewardIn != null)
                {
                    var query = _kscHrContext.RewardInSmcMonthlyProductions.Include(x => x.RewardIn)
                    .Where(x => x.RewardIn.YearMonth == yearMonth
                    && (x.ProductionEfficiencyId == EnumProductionEfficiency.zamzam.Id
                    ))
                         .Select(x => new
                         {
                             ActualWeightItem = x.ActualWeight,
                             PlanWeightItem=x.PlanWeight
                         });
                    var SumActualWeightItemByYearMonth =( query.Select(x => x.ActualWeightItem).Sum() ?? 0)/1000;
                    var SumPlanWeightItemByYearMonth = (query.Select(x => x.PlanWeightItem).Sum() ?? 0)/1000;
                     items =new Tuple<long, long>(SumActualWeightItemByYearMonth, SumPlanWeightItemByYearMonth);

                    return items;
                }
            }
            catch (Exception ex)
            {
                return items;

            }

            return items;

        }
        public long SumActualWeightZamzamVaEhyaByYearMonth(int yearMonth)
        {

            long result = 0;
            try
            {
                var RewardIn = _kscHrContext.RewardIns.Where(x => x.YearMonth == yearMonth).FirstOrDefault();
                if (RewardIn != null)
                {
                    var query = _kscHrContext.RewardInSmcMonthlyProductions.Include(x => x.RewardIn)
                    .Where(x => x.RewardIn.YearMonth == yearMonth
                    && (x.ProductionEfficiencyId == EnumProductionEfficiency.zamzam.Id
                    || x.ProductionEfficiencyId == EnumProductionEfficiency.ehya2.Id))//id=5,4                           .Select(x => new
                         .Select(x => new
                         {
                             ActualWeightItem = x.ActualWeight,
                        });
                    var SumMonthlyDriByYearMonth = query.Select(x => x.ActualWeightItem).Sum() ?? 0;

                    result = SumMonthlyDriByYearMonth;
                }
            }
            catch (Exception ex)
            {
                return result;

            }

            return result;

        }
        public long SumActualWeightZamzamVaEhyaByYearMonth1(int yearMonth)
        {

            long result = 0;
            try
            {
                var RewardIn = _kscHrContext.RewardIns.Where(x => x.YearMonth == yearMonth).FirstOrDefault();
                if (RewardIn != null)
                {
                

                 var query = _kscHrContext.RewardInSmcMonthlyProductions
                    .Include(x => x.RewardIn)
                    .Where(x => x.RewardIn.YearMonth == yearMonth
                    && x.ProductionEfficiencyId == EnumProductionEfficiency.zamzam.Id
                    && x.ProductionEfficiencyId == EnumProductionEfficiency.ehya2.Id)//id=5,4
                        .Select(x => new
                        {
                            ActualWeightItem = x.ActualWeight,
                        });
                    var SumActualWeight = query.Select(x => x.ActualWeightItem).Sum() ?? 0;


                   
                    result = SumActualWeight;
                }
            }
            catch (Exception ex)
            {
                return result;

            }

            return result;

        }



        //فرمول KPIDiv
        #region
        //PaymentAmountKpi متغیرهای 

        public double? DivPaymentAmountKpi(int yearMonth,int rewardUnitTypeId)
        {


            List<int> NotTypeIds = new List<int>(2)
                      {
                    EnumRewardUnitType.PoshtibaniPeymanKaran.Id,EnumRewardUnitType.PoshtibaniSayerNavahi.Id
                          };
            double? KpiDiv = 0;
            //var employeeCountForKpi = (Convert.ToInt64(ActiveEmployeeCount().FirstOrDefault(a => a.RewardUnitTypeId == rewardUnitTypeId)?.EmployeeCount));
            // var sumRewardSpecificEfficincyForKpi = Convert.ToInt64((SumTotalPercentPerJob().FirstOrDefault(a => a.RewardUnitTypeId == rewardUnitTypeId)?.sumRewardSpecificEfficincy));
            //KpiDiv = (double)employeeCountForKpi / sumRewardSpecificEfficincyForKpi;
            var employeeCountForKpi = Convert.ToDouble(ActiveEmployeeCount().FirstOrDefault(a => a.RewardUnitTypeId == rewardUnitTypeId)?.EmployeeCount);
            var sumRewardSpecificEfficincyForKpi = (SumTotalPercentPerJob().FirstOrDefault(a => a.RewardUnitTypeId == rewardUnitTypeId)?.sumRewardSpecificEfficincy);

            KpiDiv = (Math.Round((double)(employeeCountForKpi / sumRewardSpecificEfficincyForKpi ?? 0), 5));

            //PaymentAmountKpi = NotTypeIds.Contains(rewardUnitTypeId) ? 0 : Convert.ToInt64(item.MonthlyRewardAmount * (KpiDiv)),

            return KpiDiv;
        }



        //#region functions for insert
        ////تعداد کارکنان فعال 
        public List<EmployeeRewardSpecificDto> ActiveEmployeeCount()//(int unitTypeId)
        {
            var jobposition = _kscHrContext.Chart_JobPosition;

            List<int> paymentStatusIds = new List<int>(4)
                      {1,5,7,8};

            var query = from a in _kscHrContext.Chart_RewardSpecific
                        join t in _kscHrContext.Chart_JobPosition
                        on a.Id equals t.SpecificRewardId
                        join c in _kscHrContext.Employees
                        on t.Id equals c.JobPositionId
                        where !paymentStatusIds.Contains(c.PaymentStatusId ?? 0)

                        select new
                        {
                            c.JobPositionId,
                            a.Title,
                            a.RewardUnitTypeId,
                            c.EmployeeNumber
                        }
                        ;

            var Data = query.GroupBy(x => new
            {
                RewardUnitTypeTitle = x.Title,
                RewardUnitTypeId = x.RewardUnitTypeId,
                //JobPositionId = x.JobPositionId.Value
            }).Select(y => new EmployeeRewardSpecificDto()
            {
                //JobPositionId = y.Key.JobPositionId,
                RewardUnitTypeTitle = y.Key.RewardUnitTypeTitle,
                RewardUnitTypeId = y.Key.RewardUnitTypeId,
                EmployeeCount = y.Count(),
            });
            return Data.ToList();
            ;
        }

        //جمع ضرایب پاداش ویژه 

        public List<EmployeeRewardSpecificDto> SumTotalPercentPerJob()//(int unitTypeId)
        {
            List<int> paymentStatusIds = new List<int>(4)
                      {1,5,7,8};

            var query =
                 //from a in _kscHrContext.Chart_RewardSpecific
                 //        join t in _kscHrContext.Chart_JobPosition
                 //        on a.Id equals t.SpecificRewardId
                from a in _kscHrContext.Chart_RewardSpecific
                             join t in _kscHrContext.Chart_JobPosition
                             on a.Id equals t.SpecificRewardId
                             join c in _kscHrContext.Employees
                             on t.Id equals c.JobPositionId
                             where !paymentStatusIds.Contains(c.PaymentStatusId ?? 0)

                //ستون های نمایشی
                select new
                {
                    a.RewardUnitTypeId,
                    t.RewardSpecificEfficincy,
                }
                        ;

            var Data = query.GroupBy(x => new
            {
                RewardUnitTypeId = x.RewardUnitTypeId,
            }).Select(y => new EmployeeRewardSpecificDto()
            {
                RewardUnitTypeId = y.Key.RewardUnitTypeId,
                sumRewardSpecificEfficincy = y.Sum(s => (s.RewardSpecificEfficincy ?? 0)),


            });
            return Data.ToList();

            ;

        }
        #endregion
    }
}

