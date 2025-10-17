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
    public interface IMission_TypeAccessLevelRepository : IRepository<Mission_TypeAccessLevel, int>
    {
        bool CheckAccessLevelUser(List<string> roles);
        IQueryable<Mission_TypeAccessLevel> GetAllAccessLevelsNoTracking();
        IQueryable<Mission_TypeAccessLevel> GetAllMissionTypeAccessLevelsNoTracking();
    }
}
