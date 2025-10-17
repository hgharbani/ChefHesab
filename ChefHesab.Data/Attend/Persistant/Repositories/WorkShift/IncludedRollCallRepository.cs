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
    public class IncludedRollCallRepository : EfRepository<IncludedRollCall, int>, IIncludedRollCallRepository
    {
        private readonly KscHrContext _kscHrContext;
        public IncludedRollCallRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;

        }

        public IQueryable<IncludedRollCall> GetAllByRelated()
        {

            var query = _kscHrContext.IncludedRollCalls.Where(x => x.IsActive == true).Include(a => a.RollCallDefinition).AsQueryable().AsNoTracking();
            return query;
        }
        public IQueryable<IncludedRollCall> GetActiveIncludedRollCallByIncludedIdAsNoTracking(int includedDefinitionId)
        {

            var query = _kscHrContext.IncludedRollCalls.Where(x => x.IsActive == true && x.IncludedDefinitionId == includedDefinitionId).AsNoTracking();
            return query;
        }

        public IQueryable<IncludedRollCall> GetAllAsNoTrackingByIncludedDefinition()
        {

            var query = _kscHrContext.IncludedRollCalls.Where(x => x.IsActive == true).Include(a => a.IncludedDefinition).AsQueryable().AsNoTracking();
            return query;
        }

        public List<int> GetRollCallDefinitionByIncludedDefinitionCode(string code)
        {
            var includedDefinitions = _kscHrContext.IncludedDefinitions.AsNoTracking().Where(a => a.Code == code).First();
            var inCludedRollCall = _kscHrContext.IncludedRollCalls.AsNoTracking()
                .Where(c => includedDefinitions.Id == c.IncludedDefinitionId && c.IsActive == true)
                .Select(a => a.RollCallDefinitionId).ToList();
            return inCludedRollCall;
        }

    }
}
