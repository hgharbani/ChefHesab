using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.CompatibleRollCall;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface ICompatibleRollCallRepository : IRepository<CompatibleRollCall, int>
    {
        IQueryable<CompatibleRollCall> GetCompatibleRollCallByCompatibleRollCallType(int CompatibleRollCallTypeId);
        IQueryable<CompatibleRollCallByCompatibleTypeModel> GetCompatibleRollCallByCompatibleTypeAsNoTracking(int compatibleRollCallType);
    }
}
