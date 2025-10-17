using KSC.Domain;
using Ksc.HR.Domain.Entities.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Rule;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface IInterdictWeatherRepository : IRepository<InterdictWeather, int>
    {
        IQueryable<InterdictWeather> GetWeatherWithCityAndCategoryDefination(int workCityId, int yearmonth, int jobCategoryDefinationId);
    }
}
