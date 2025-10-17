using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Ksc.HR.Share.Model.Reward;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardBaseSpecificOfUnitHeaderRepository : EfRepository<RewardBaseSpecificOfUnitHeader, int>, IRewardBaseSpecificOfUnitHeaderRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardBaseSpecificOfUnitHeaderRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public RewardBaseSpecificOfUnitHeader GetRewardBaseSpecificOfUnitHeaderByYearMonth(int yearMonth)
        {
            var query = _kscHrContext.RewardBaseSpecificOfUnitHeaders
                .Include(x => x.RewardBaseSpecificOfUnitDetails)
                //.Where(x => x. == yearMonth);// && (x.ValidEndDate >= yearMonth || !x.ValidEndDate.HasValue))
                .FirstOrDefault()
                ;

            return query;
        }
        public int GetHeaderIdBybasisId(int YearMonth, int rewardUnitTypeId, bool IsMonthly)
        {
            var query = _kscHrContext.RewardBaseSpecificOfUnitHeaders.Where(x => x.YearMonth == YearMonth &&
            x.RewardUnitTypeId == rewardUnitTypeId
            &&
            x.IsMonthly == IsMonthly
            ).Select(x => x.Id).FirstOrDefault();
            return query;
        }

        //public IQueryable<RewardBaseSpecificOfUnitHeader>  GetRewardBaseSpecificOfUnitHeaderByRelated(int yearMonth,int rewardUnitTypeId)
        //{
        //    var query = _kscHrContext.RewardBaseSpecificOfUnitHeaders

        //        .Include(x => x.RewardBaseSpecificOfUnitDetails)
        //        .Include(x => x.RewardUnitType)
        //        .Where(x=>x.RewardUnitTypeId == rewardUnitTypeId
        //        && x.RewardBaseSpecific.ValidStartDate==yearMonth
        //        )   

        //        // .FirstOrDefault();
        //        //.Where(x => x.RewardBaseSpecificOfUnitDetails.Any(a=>a.IsMonthly==0))
        //        .AsNoTracking();
        //    return query;
        //}

        public IQueryable<RewardBaseSpecificOfUnitHeader> GetRewardBaseSpecificOfUnitHeaderByRelated(int yearMonth, int rewardUnitTypeId)
        {
            var query = _kscHrContext.RewardBaseSpecificOfUnitHeaders.Where(a => a.RewardUnitTypeId == rewardUnitTypeId
            && a.IsMonthly == false
            && a.YearMonth == yearMonth)
                .Include(x => x.RewardBaseSpecificOfUnitDetails)
                .Include(x => x.RewardBaseSpecific)
                .Where(x => x.RewardBaseSpecific.IsActive == true && x.RewardBaseSpecific.ValidStartDate == yearMonth)
                .AsNoTracking()
                ;

            return query;
        }

        public IQueryable<RewardBaseSpecificOfUnitHeader> GetRewardBaseSpecificDetailByRelated(int yearMonth, int rewardUnitTypeId)
        {
            var query = _kscHrContext.RewardBaseSpecificOfUnitHeaders.Where(a => a.RewardUnitTypeId == rewardUnitTypeId && a.IsMonthly == false)
                .Include(x => x.RewardBaseSpecificOfUnitDetails)
                .Include(x => x.RewardBaseSpecific)
                .Where(x => x.RewardBaseSpecific.IsActive == true
                && (x.RewardBaseSpecific.ValidStartDate <= yearMonth && (x.RewardBaseSpecific.ValidEndDate >= yearMonth || !x.RewardBaseSpecific.ValidEndDate.HasValue))
                )
                //.Where(x => x.ValidStartDate <= yearMonth && (x.ValidEndDate >= yearMonth || !x.ValidEndDate.HasValue))

                .AsNoTracking()
                ;

            return query;
        }

        /// <summary>
        /// جستجو واحد و سال ماه در جدول header
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <param name="rewardUnitTypeId"></param>
        /// <returns></returns>
        public RewardBaseSpecificOfUnitHeader GetLatestDataForFinal(int yearMonth, int rewardUnitTypeId)
        {
            var result = new RewardBaseSpecificOfUnitHeader();
            // جستجو به ازای سال ماه انتخابی
            var query = _kscHrContext.RewardBaseSpecificOfUnitHeaders
                .Where(a => a.RewardUnitTypeId == rewardUnitTypeId && a.YearMonth == yearMonth && a.IsMonthly == true) //واحد انتخابی و سال ماه
                .Include(x => x.RewardBaseSpecificOfUnitDetails)
                .Include(x => x.RewardUnitType)
                .Include(x => x.RewardBaseSpecific)
                //.Where(x => x.RewardBaseSpecific.IsActive == true && x.RewardBaseSpecific.ValidEndDate.HasValue == false) //اخرین دیتا فعال
                .AsNoTracking()
                .AsQueryable()
                ;


            var headerByYearMonth = query.FirstOrDefault();
            if (headerByYearMonth != null)
            {// در صورت وجود دیتا نمایش مستقیما از دیتا بیس
                result = headerByYearMonth;
            }
            else
            {//اگر دیتا موجود نبود
                //آخرین دیتا سال ماه موجود واحد انتخابی را واکشی میکند جهت استفاده از حداقل و حداکثر
                var headersActive = _kscHrContext.RewardBaseSpecificOfUnitHeaders
                .Where(a => a.RewardUnitTypeId == rewardUnitTypeId && a.IsMonthly == true) //واحد انتخابی
                .Include(x => x.RewardUnitType)
                .Include(x => x.RewardBaseSpecific)
                //.Where(x => x.RewardBaseSpecific.IsActive == true && x.RewardBaseSpecific.ValidEndDate.HasValue == false) //اخرین دیتا فعال
                .Include(x => x.RewardBaseSpecificOfUnitDetails)
                .AsNoTracking()
                .AsQueryable()
                ;

                //// با توجه به دیتایی سال ماه پیدا کن که جدول جزییات رکورد داشته باشد = مشکل فولاد سازی در تابستان
                var maxYearMonth = headersActive.Where(x => x.RewardBaseSpecificOfUnitDetails.Count() != 0).Max(x => x.YearMonth);

                var headerByMaxYearMonth = headersActive.FirstOrDefault(x => x.YearMonth == maxYearMonth);
                result = headerByMaxYearMonth;

            }


            return result;
        }


        public Tuple<List<RewardBaseSpecificOfUnitHeader>, RewardBaseSpecific, List<MaxYearMonthHeaderByUnitVM>> GetLatestDataForCreate(List<int> rewardUnitTypeId)
        {
            var rewardBaseSpecificLatest = _kscHrContext.RewardBaseSpecifics
                .FirstOrDefault(x => x.ValidEndDate.HasValue == false && x.IsActive == true); //اخرین دیتا فعال

            //آخرین دیتا سال ماه موجود واحد انتخابی را واکشی میکند 
            var headersLatestIsMonthlyFalse = _kscHrContext.RewardBaseSpecificOfUnitHeaders
            .Where(a => a.RewardBaseSpecificId == rewardBaseSpecificLatest.Id
             && rewardUnitTypeId.Contains(a.RewardUnitTypeId)
             && a.IsMonthly == false
            )
            .Include(x => x.RewardBaseSpecificOfUnitDetails)
            .ToList();

            var maxYearMonthHeaderByUnit = _kscHrContext.RewardBaseSpecificOfUnitHeaders
            .Where(a => a.RewardBaseSpecificId == rewardBaseSpecificLatest.Id
             && rewardUnitTypeId.Contains(a.RewardUnitTypeId))
             .GroupBy(x => x.RewardUnitTypeId)
             .Select(x => new MaxYearMonthHeaderByUnitVM
             {
                 UnitTypeId = x.Key,
                 MaxYearMonthHeader = x.Max(b => b.YearMonth.Value)
             }).ToList();

            var result = new Tuple<List<RewardBaseSpecificOfUnitHeader>,
                RewardBaseSpecific,
                List<MaxYearMonthHeaderByUnitVM>>
                (headersLatestIsMonthlyFalse,
                rewardBaseSpecificLatest,
                maxYearMonthHeaderByUnit
                );

            return result;
        }

        public int GetMaxYearMonth(int yearMonth, int rewardUnitTypeId)
        {
            var query = _kscHrContext.RewardBaseSpecificOfUnitHeaders
                .Where(a => a.RewardUnitTypeId == rewardUnitTypeId) // واحد 
                .Include(x => x.RewardBaseSpecific)
                .Where(x => x.RewardBaseSpecific.IsActive == true
                && (x.RewardBaseSpecific.ValidStartDate <= yearMonth && (x.RewardBaseSpecific.ValidEndDate >= yearMonth || !x.RewardBaseSpecific.ValidEndDate.HasValue))
                )
                ;
            int result = query.Max(x => x.YearMonth.Value);
            return result;
        }
    }
}

