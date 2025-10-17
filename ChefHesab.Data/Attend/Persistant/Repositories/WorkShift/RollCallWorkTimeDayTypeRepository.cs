using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class RollCallWorkTimeDayTypeRepository : EfRepository<RollCallWorkTimeDayType, int>, IRollCallWorkTimeDayTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RollCallWorkTimeDayTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<RollCallWorkTimeDayType> GetRollCallWorkTimeDayTypeAsNoTracking()
        {
            return _kscHrContext.RollCallWorkTimeDayTypes.AsNoTracking();
        }
        public IQueryable<RollCallWorkTimeDayType> GetIncludedRollCallWorkTimeDayTypeRollCallDefinitionSalaryCodes()
        {
            return _kscHrContext.RollCallWorkTimeDayTypes.Include(x => x.RollCallDefinition).ThenInclude(x => x.RollCallSalaryCodes);
        }
        public IQueryable<RollCallWorkTimeDayType> GetRollCallWorkTimeDayTypeByRollCallDefinitionId(int rollCallDefinitionId)
        {
            return _kscHrContext.RollCallWorkTimeDayTypes.Where(x => x.RollCallDefinitionId == rollCallDefinitionId);
        }
    }
}
