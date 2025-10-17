using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Ksc.HR.Share.Model.Reward;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using System.Globalization;
using KSC.Common.Filters.Models;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardInMonthlyProductionSaleRepository : EfRepository<RewardInMonthlyProductionSale, int>, IRewardInMonthlyProductionSaleRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardInMonthlyProductionSaleRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        //public double GetToolidMoadel(int yearMonth)
        //{
        //    return 344930.185;
        //    long result = 0;
        //    var MonthlyProductionSale = _kscHrContext.RewardInMonthlyProductionSales
        //        .Include(x => x.RewardIn)
        //        .Where(x => x.RewardIn.YearMonth == yearMonth).AsQueryable();
        //    if (MonthlyProductionSale.Any())
        //    {
        //        var MonthlyProductionSales = MonthlyProductionSale.Where(x => x.IsgPercent != 0)
        //            .Select(x => new
        //            {
        //                x.IsgPercent,
        //                x.IsgPlanWeight,
        //                PlanWeightPercent = x.IsgPlanWeight * x.IsgPercent,
        //            }).ToList();
        //        var SumPlanWeightPercent = MonthlyProductionSales.Select(x => x.PlanWeightPercent).Sum() ?? 0;

        //        result = (long)(SumPlanWeightPercent / 1000);
        //    }
        //    return result;

        //}


        /// <summary>
        /// اعداد لازم جهت محاسبه تولید معادل
        /// استفاده در جداول مبنا کارانه تولید
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public ClacToolidMoadelViewModel GetClacToolidMoadelViewModel(int yearMonth)
        {
            ClacToolidMoadelViewModel result = new ClacToolidMoadelViewModel { YearMonth = yearMonth };

            var MonthlyProductionSaleWithZero = _kscHrContext.RewardInMonthlyProductionSales
                .Include(x => x.RewardIn)
                .Where(x => x.RewardIn.YearMonth == yearMonth)
                .Include(a => a.RewardIn).ThenInclude(a => a.RewardInQualityControlMonthlyProductions)
                .AsQueryable().AsNoTracking().ToList();

            if (MonthlyProductionSaleWithZero.Any())
            {
                var MonthlyProductionSales = MonthlyProductionSaleWithZero.Where(x => x.IsgPercent != 0 && x.IsgPercent != null).Select(x => new
                {
                    x.IsgPercent, // ضریب ISG
                    x.IsgPlanWeight, // تناژ برنامه ریزی
                    PlanWeightPercent = Convert.ToDouble(x.IsgPlanWeight) * x.IsgPercent,
                    ////////////////
                    x.IsgActualWeight, // تناژ واقعی
                    justfloatIsgPercent = Math.Round(x.IsgPercent.Value - Math.Truncate(x.IsgPercent.Value), 3),
                    KomakiTonazh = x.IsgActualWeight >= x.IsgPlanWeight ? x.IsgPlanWeight : x.IsgActualWeight,

                }).ToList();
                var SumPlanWeightPercent = MonthlyProductionSales.Sum(x => x.PlanWeightPercent) ?? 0;

                result.one = (long)SumPlanWeightPercent;

                var SumPlanWeight = MonthlyProductionSales.Sum(x => x.IsgPlanWeight);
                SumPlanWeight = SumPlanWeight.HasValue ? (SumPlanWeight.Value != 0 ? SumPlanWeight.Value : 1) : 1;//چون مخرجه در صورت خالی بودن و صفر بودن یک شده
                result.two = ((double)result.one / (double)SumPlanWeight);
                // result.two = Convert.ToDouble(GetDecimalNumberWithDecimalCount(result.two.ToString(), 7));
                result.two = FloorToDecimalPlaces(result.two, 7);
                result.three = FloorToDecimalPlaces(0.07 / (result.two - 1), 7); 
                //result.three = Math.Round(0.07 / (result.two - 1), 7);
                //result.three = (0.07) / (result.two - 1);

                // آرایه کمکی تناژ
                List<double?> ArrayKomakiTonazh = new List<double?>();
                foreach (var item in MonthlyProductionSales)
                {
                    var CalKomakiAndJustFloat = (item.justfloatIsgPercent * item.KomakiTonazh.Value) * result.three;
                    ArrayKomakiTonazh.Add(CalKomakiAndJustFloat);
                }
                var SumActual = MonthlyProductionSaleWithZero.Sum(x => x.IsgActualWeight);
                var SumArrayKomakiTonazh = ArrayKomakiTonazh.Sum(x => x);
                var NotOkWeight = MonthlyProductionSaleWithZero.FirstOrDefault().RewardIn.RewardInQualityControlMonthlyProductions
                    .FirstOrDefault().NotOkWeight;

                result.ToolidMoadel = ((SumActual ?? 0) + (SumArrayKomakiTonazh ?? 0)) - (NotOkWeight ?? 0);

                result.ToolidMoadel = (double)result.ToolidMoadel / 1000;
                result.ToolidMoadel = Math.Round(result.ToolidMoadel, 7);



            }

            return result;

        }
        public string GetDecimalNumberWithDecimalCount(string str, int count)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            var seprator = culture.NumberFormat.NumberDecimalSeparator;
            if (str.Count() == 1) return "0";
            var GEtNumber = str.Split(seprator.ToCharArray());
            return $"{GEtNumber[0]}.{GEtNumber[1].Substring(0, count)}";
        }
        public double FloorToDecimalPlaces(double value, int decimalPlaces)
        {
            double multiplier = Math.Pow(10, decimalPlaces);
            return Math.Floor(value * multiplier) / multiplier;
        }


        //دیتای خام تولید روزانه به ازای rewardin که سال/ماه دارد
        public IQueryable<RewardInDailyProductionSale> GetRewardInDailyProductionSale(int yearmonth,int rewardUnitTypeId)
        {
            var query = _kscHrContext.RewardInDailyProductionSales.Include(x => x.RewardIn)
                .Where(x => x.RewardIn.YearMonth == yearmonth && x.RewardUnitTypeId== rewardUnitTypeId)
                ;

            return query;
        }

    }
}

