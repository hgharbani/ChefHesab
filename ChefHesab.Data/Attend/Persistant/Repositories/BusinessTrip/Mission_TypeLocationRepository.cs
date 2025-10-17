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
using Ksc.HR.Domain.Repositories;

namespace Ksc.HR.Data.Persistant.Repositories.BusinessTrip
{
    public class Mission_TypeLocationRepository : EfRepository<Mission_TypeLocation, int>, IMission_TypeLocationRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Mission_TypeLocationRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<Mission_TypeLocation> GetActiveMission_TypeLocationIncluded()
        {
            return _kscHrContext.Mission_TypeLocations.Where(x => x.IsActive).Include(x => x.Mission_Type).Include(x => x.Mission_Location);
        }
        public IQueryable<Mission_TypeLocation> GetAllMission_TypeLocationIncluded()
        {
            return _kscHrContext.Mission_TypeLocations.Include(x => x.Mission_Type).Include(x => x.Mission_Location);
        }
        public Mission_TypeLocation GetActiveMissionByTypeLocation(int missionTypeId, int missionLocationId)
        {
            return _kscHrContext.Mission_TypeLocations.FirstOrDefault(x => x.IsActive && x.MissionTypeId == missionTypeId && x.MissionLocationId == missionLocationId);
        }
        public ValueTuple<Mission_TypeLocation, int?, int?> GetMissionMaximumDay(int missionTypeId, int missionLocationId, int workCityId)
        {
            int? missionMaximumDay = null;
            int? missionMinimumDay = null;
            var mission_TypeLocation = _kscHrContext.Mission_TypeLocations
                .Where(x => x.IsActive && x.MissionLocationId == missionLocationId && x.MissionTypeId == missionTypeId)
                .Include(x => x.Mission_Type).Include(x => x.Mission_Location)
                .Include(x => x.Mission_TypeLocationWorkCities).FirstOrDefault();
            if (mission_TypeLocation != null)
            {
                if (mission_TypeLocation.Mission_TypeLocationWorkCities.Any(x => x.IsActive && x.WorkCityId == workCityId))
                {
                    var missionDataByCity = mission_TypeLocation.Mission_TypeLocationWorkCities.FirstOrDefault(x => x.IsActive && x.WorkCityId == workCityId);
                    //var missionMaximumDayByCity = missionDataByCity.MissionMaximumDay;
                    if (missionDataByCity != null)
                    {
                        missionMaximumDay = missionDataByCity.MissionMaximumDay;
                        //   missionMinimumDay = missionDataByCity.MissionMinimumDayForChecked;
                    }

                }
                if (missionMaximumDay == null)
                    missionMaximumDay = mission_TypeLocation.MissionMaximumDay;
                //if (missionMinimumDay == null)
                //    missionMinimumDay = mission_TypeLocation.MissionMinimumDayForChecked;
            }
            //
            var missionLocation = _kscHrContext.Mission_Locations.FirstOrDefault(x => x.Id == missionLocationId);
            missionMinimumDay = missionLocation.MaximumMissionDayForPayment + 1; // حتما باید با یک جمع شود
            //missionMinimumDay = اگر تعداد روزهای ماموریت برابر این فیلد یا بیشتر باشد،حق ماموریت پرداخت نمیشود
            //
            return new ValueTuple<Mission_TypeLocation, int?, int?>(mission_TypeLocation, missionMaximumDay, missionMinimumDay);
        }
        public ValueTuple<int, bool> GetActiveMissionByTypeLocationWorkCity(int missionTypeId, int missionLocationId, int workCityId)
        {
            var mission_TypeLocation = _kscHrContext.Mission_TypeLocations
                .Where(x => x.IsActive && x.MissionLocationId == missionLocationId && x.MissionTypeId == missionTypeId)
                .Include(x => x.Mission_Type).Include(x => x.Mission_Location)
                .Include(x => x.Mission_TypeLocationWorkCities).FirstOrDefault();
            int missionMinimumDayForChecked = 0;
            bool checkMissionDaysInMonth = false;
            if (mission_TypeLocation != null)
            {
                if (mission_TypeLocation.Mission_TypeLocationWorkCities.Any(x => x.IsActive && x.WorkCityId == workCityId))
                {
                    var mission_TypeLocationWorkCity = mission_TypeLocation.Mission_TypeLocationWorkCities.FirstOrDefault(x => x.IsActive && x.WorkCityId == workCityId);
                    if (mission_TypeLocationWorkCity != null)
                    {
                        missionMinimumDayForChecked = mission_TypeLocationWorkCity.MissionMinimumDayForChecked;
                        checkMissionDaysInMonth = mission_TypeLocationWorkCity.CheckMissionDaysInMonth;
                    }

                }
                else
                {
                    missionMinimumDayForChecked = mission_TypeLocation.MissionMinimumDayForChecked;
                    checkMissionDaysInMonth = mission_TypeLocation.CheckMissionDaysInMonth;
                }

            }
            return new ValueTuple<int, bool>(missionMinimumDayForChecked, checkMissionDaysInMonth);
        }
        public Mission_TypeLocation GetMission_TypeLocationById(int id)
        {
            return _kscHrContext.Mission_TypeLocations.Include(x => x.Mission_Location).Include(x => x.Mission_Type).FirstOrDefault(x => x.Id == id);
        }
    }
}
