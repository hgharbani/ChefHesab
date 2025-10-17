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

namespace Ksc.HR.Data.Persistant.Repositories.BusinessTrip
{
    public class Mission_TypeRepository : EfRepository<Mission_Type, int>, IMission_TypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Mission_TypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<Mission_Type> GetAllMissionTypeNoTracking()
        {
            var result = _kscHrContext.Mission_Types.Include(x => x.Mission_TypeAccessLevels).ThenInclude(x=>x.AccessLevel).AsNoTracking();
            return result;
        }
    }
}
