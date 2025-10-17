using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Rule;
using KSC.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class InterdictWeatherRepository : EfRepository<InterdictWeather, int>, IInterdictWeatherRepository
    {
        private readonly KscHrContext _kscHrContext;
        public InterdictWeatherRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<InterdictWeather> GetWeatherWithCityAndCategoryDefination(int workCityId, int yearmonth, int jobCategoryDefinationId)
        {
            var weader = _kscHrContext.InterdictWeather.Where(x => x.WorkCityId == workCityId && x.StartDate <= yearmonth && x.EndDate >= yearmonth && x.JobCategoryDefinationId == jobCategoryDefinationId);
            return weader;
        }

    }
}
