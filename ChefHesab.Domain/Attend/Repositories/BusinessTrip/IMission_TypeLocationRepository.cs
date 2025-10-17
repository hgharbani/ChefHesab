using KSC.Domain;
using Ksc.HR.Domain.Entities.HRSystemStatusControl;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.BusinessTrip;

namespace Ksc.HR.Domain.Repositories.BusinessTrip
{
    public interface IMission_TypeLocationRepository : IRepository<Mission_TypeLocation, int>
    {
        Mission_TypeLocation GetActiveMissionByTypeLocation(int missionTypeId, int missionLocationId);
        (int, bool) GetActiveMissionByTypeLocationWorkCity(int missionTypeId, int missionLocationId, int workCityId);
        IQueryable<Mission_TypeLocation> GetActiveMission_TypeLocationIncluded();
        IQueryable<Mission_TypeLocation> GetAllMission_TypeLocationIncluded();
        (Mission_TypeLocation, int?, int?) GetMissionMaximumDay(int missionTypeId, int missionLocationId, int workCityId);
        Mission_TypeLocation GetMission_TypeLocationById(int id);
    }
}
