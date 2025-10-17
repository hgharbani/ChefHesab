using KSC.Domain;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.OnCall

{
    public interface IOnCall_TypeRepository : IRepository<OnCall_Type, int>
    {
        IQueryable<OnCall_Type> GetAllOnCallTypeNoTracking();
        IEnumerable<OnCall_Type> GetOnCallTypeByRealtedEntities();
    }
}
