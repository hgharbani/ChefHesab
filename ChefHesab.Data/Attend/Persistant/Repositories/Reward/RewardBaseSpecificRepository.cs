using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Share.Model.Reward;
using Ksc.HR.Share.Model.Rule;
using System.Linq;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardBaseSpecificRepository : EfRepository<RewardBaseSpecific, int>, IRewardBaseSpecificRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardBaseSpecificRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public int GetIdByyear(int Year)
        {
            var query = _kscHrContext.RewardBaseSpecifics.Where(x => x.Year == Year ).Select(x => x.Id).FirstOrDefault();
            return query;
        }

        public IQueryable<RewardBaseSpecific> GetRewardBaseSpecifics(int? yearMonth, int? unitTypeIdInHeader, bool asNoTracking = false, string include = null)
        {
            var query = _kscHrContext.RewardBaseSpecifics.Where(x => x.IsActive).AsQueryable();
            if (asNoTracking)
                query = query.AsNoTracking();

            if (yearMonth.HasValue && yearMonth.Value > 0)
                query = query.Where(x => x.ValidStartDate <= yearMonth && (x.ValidEndDate >= yearMonth || !x.ValidEndDate.HasValue));

            if (yearMonth.HasValue && yearMonth.Value == 0) // برای یافتن اخرین رکورد جدول RewardBaseSpecific
                query = query.Where(x => x.ValidEndDate.HasValue == false);

            if (!string.IsNullOrEmpty(include))
            {
                if (include.ToLower() == "Header".ToLower())
                {
                    if (unitTypeIdInHeader.HasValue)
                    {
                        query = query.Include(x => x.RewardBaseSpecificOfUnitHeaders
                        .Where(x => x.RewardUnitTypeId == unitTypeIdInHeader && x.YearMonth == yearMonth));

                    }
                    else
                    {
                        query = query.Include(x => x.RewardBaseSpecificOfUnitHeaders).Where(x => x.IsActive);
                    }

                }
                if (include.ToLower() == "HeaderDetail".ToLower())
                {
                    if (unitTypeIdInHeader.HasValue)
                    {
                        query = query.Include(x => x.RewardBaseSpecificOfUnitHeaders
                        .Where(x =>  x.RewardUnitTypeId == unitTypeIdInHeader && x.YearMonth == yearMonth))
                            .ThenInclude(x => x.RewardBaseSpecificOfUnitDetails);

                    }
                    else
                    {
                        query = query.Include(x => x.RewardBaseSpecificOfUnitHeaders).ThenInclude(x => x.RewardBaseSpecificOfUnitDetails);
                    }
                }
            }
            return query;
        }



        //public RewardBaseSpecific GetLatest(int? unitTypeIdInHeader, bool asNoTracking = false, string include = null)
        //{
        //    var query = GetRewardBaseSpecifics(null, unitTypeIdInHeader, asNoTracking, include).Where(x => x.ValidEndDate.HasValue == false);

        //    var result = query.FirstOrDefault();
        //    return result;
        //}

       
        public IQueryable<RewardBaseSpecific>  GetRewardBaseSpecificByYearMonth(int yearMonth)
        {
            var query = _kscHrContext.RewardBaseSpecifics
                //.Include(x => x.RewardBaseSpecificOfUnitHeaders)
                // .ThenInclude(x => x.RewardBaseSpecificOfUnitDetails)
                .Where(x => x.ValidStartDate <= yearMonth && (x.ValidEndDate >= yearMonth || !x.ValidEndDate.HasValue)
                && x.IsActive==true
                )
                //.AsNoTracking();
                ;

            return query;
        }

        public IQueryable<RewardBaseSpecific> GetRewardBaseSpecificByRelated(int yearMonth, int rewardUnitTypeId)
        {
            var query = _kscHrContext.RewardBaseSpecifics
                .Include(x => x.RewardBaseSpecificOfUnitHeaders.Where(a => a.RewardUnitTypeId == rewardUnitTypeId && a.IsMonthly == false ))
                 .ThenInclude(x => x.RewardBaseSpecificOfUnitDetails)
                .Where(x => x.ValidStartDate == yearMonth)
                .AsNoTracking()
                ;

            return query;
        }
   

    }
}

