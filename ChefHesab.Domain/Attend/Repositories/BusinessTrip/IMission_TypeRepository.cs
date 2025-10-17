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
    public interface IMission_TypeRepository : IRepository<Mission_Type, int>
    {
        //IQueryable<Mission_Type> GetAllMissionTypeNoTracking();
        IQueryable<Mission_Type> GetAllMissionTypeNoTracking();
    }
}
