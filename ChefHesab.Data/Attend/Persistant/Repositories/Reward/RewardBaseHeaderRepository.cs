using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Share.Model.Reward;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardBaseHeaderRepository : EfRepository<RewardBaseHeader, int>, IRewardBaseHeaderRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardBaseHeaderRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public RewardBaseHeader GetRewardBaseHeaderByYearMonth(int yearMonth)
        {
            var query = _kscHrContext.RewardBaseHeaders
                .Include(x => x.RewardBaseDetails)
                .Where(x => x.ValidStartDate <= yearMonth && (x.ValidEndDate >= yearMonth || !x.ValidEndDate.HasValue))
                .FirstOrDefault()
                ;

            return query;
        }

        public IQueryable<RewardBaseHeader> GetRewardBaseHeaders(RewardBaseHeaderSearchModel searchModel)
        {
            var query = _kscHrContext.RewardBaseHeaders.AsQueryable();
            if (searchModel != null)
            {
                if (searchModel.Id.HasValue)
                    query = query.Where(x => x.Id == searchModel.Id);
                if (searchModel.Year.HasValue)
                    query = query.Where(x => x.Year == searchModel.Year);
                if (searchModel.YearMonth.HasValue)
                    query = query.Where(x => x.ValidStartDate <= searchModel.YearMonth && (x.ValidEndDate >= searchModel.YearMonth || !x.ValidEndDate.HasValue));

            }
            return query;
        }

        /// <summary>
        /// رکوردی که تاریخ پایان نداشته باشد : آخرین دیتای ثبت شده
        /// استفاده در فیلتر تاریخ جدوال مبنا کارانه
        /// </summary>
        /// <returns>AsNoTracking</returns>
        public RewardBaseHeader GetLatest()
        {
            RewardBaseHeaderSearchModel searchModel = new RewardBaseHeaderSearchModel();
            var result = GetRewardBaseHeaders(searchModel).AsNoTracking().Where(x => x.ValidEndDate.HasValue == false).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// یاقتن رکورد بر اساس سال ماه 
        /// </summary>
        /// <param name="yearMonth">YYYYMM</param>
        /// <returns>AsNoTracking</returns>
        public RewardBaseHeader GetByValidStartDate(int? yearMonth)
        {
            RewardBaseHeader result = new RewardBaseHeader();
            RewardBaseHeaderSearchModel searchModel = new RewardBaseHeaderSearchModel { YearMonth = yearMonth };
            if (yearMonth.HasValue)
                result = GetRewardBaseHeaders(searchModel).AsNoTracking().FirstOrDefault();
            else
                result = GetRewardBaseHeaders(searchModel).AsNoTracking().Where(x => x.ValidEndDate.HasValue == false).FirstOrDefault();
            return result;
        }

        public RewardBaseHeader GetByValidStartDateQuery(int? yearMonth)
        {
            RewardBaseHeader result = new RewardBaseHeader();
            RewardBaseHeaderSearchModel searchModel = new RewardBaseHeaderSearchModel { YearMonth = yearMonth };
            if (yearMonth.HasValue)
                result = GetRewardBaseHeaders(searchModel).FirstOrDefault();
            else
                result = GetRewardBaseHeaders(searchModel).Where(x => x.ValidEndDate.HasValue == false).FirstOrDefault();
            return result;
        }
    }
}

