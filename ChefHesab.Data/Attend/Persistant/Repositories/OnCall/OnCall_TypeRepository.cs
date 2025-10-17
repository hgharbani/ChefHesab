using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Repositories.OnCall;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.OnCall
{
    public class OnCall_TypeRepository : EfRepository<OnCall_Type, int>, IOnCall_TypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public OnCall_TypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext= KscHrContext;
        }
        public IEnumerable<OnCall_Type> GetOnCallTypeByRealtedEntities()
        {
            return GetAll().AsQueryable().Include(x => x.OnCall_AcessManagmentOnCallTypes);
        }
        public IQueryable<OnCall_Type> GetAllOnCallTypeNoTracking()
        {
            return _kscHrContext.OnCall_Types.AsNoTracking();
        }


    }
}
