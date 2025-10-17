using Ksc.HR.Domain.Entities.BusinessTrip;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.BusinessTrip
{
    public interface IMission_TypeLocationWorkCityRepository : IRepository<Mission_TypeLocationWorkCity, int>
    {
        IQueryable<Mission_TypeLocationWorkCity> GetAllByMissionTypeLocationId(int missionTypeLocationId);
    }
}
