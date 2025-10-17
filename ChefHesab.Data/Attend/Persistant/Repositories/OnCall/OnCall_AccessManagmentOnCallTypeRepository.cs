using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Repositories.OnCall;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.OnCall
{
    public class OnCall_AccessManagmentOnCallTypeRepository : EfRepository<OnCall_AcessManagmentOnCallType, int>, IOnCall_AccessManagmentOnCallTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public OnCall_AccessManagmentOnCallTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IEnumerable<OnCall_AcessManagmentOnCallType> GetAccessManagmentOnCallTypeByRelated()
        {
            return GetAll().AsQueryable().Include(x => x.OnCall_Type);
        }
    }
}
