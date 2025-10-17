using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.Domain.Repositories.ODSViews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.BusinessTrip;
using Ksc.HR.Domain.Repositories.BusinessTrip;

namespace Ksc.HR.Data.Persistant.Repositories.BusinessTrip
{
    public class Mission_TypeLocationWorkCityRepository : EfRepository<Mission_TypeLocationWorkCity, int>, IMission_TypeLocationWorkCityRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Mission_TypeLocationWorkCityRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<Mission_TypeLocationWorkCity> GetAllByMissionTypeLocationId(int missionTypeLocationId)
        {
            var query = _kscHrContext.Mission_TypeLocationWorkCities.Where(x => x.MissionTypeLocationId == missionTypeLocationId)
                .Include(x => x.Mission_TypeLocation).ThenInclude(x => x.Mission_Type)
                .Include(x => x.Mission_TypeLocation).ThenInclude(x => x.Mission_Location)
                .Include(x => x.WorkCity).ThenInclude(x => x.City);
            return query;
        }
    }
}
