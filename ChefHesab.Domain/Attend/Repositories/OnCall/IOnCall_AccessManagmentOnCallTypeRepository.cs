using Ksc.HR.Domain.Entities.Oncall;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.OnCall
{
    public interface IOnCall_AccessManagmentOnCallTypeRepository : IRepository<OnCall_AcessManagmentOnCallType, int>
    {
        IEnumerable<OnCall_AcessManagmentOnCallType> GetAccessManagmentOnCallTypeByRelated();
    }
}
