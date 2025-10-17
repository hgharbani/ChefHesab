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
    public class WorkGroupRepository : EfRepository<WorkGroup, int>, IWorkGroupRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WorkGroupRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public async Task<WorkGroup> GetWorkGroupByRelations(int id)
        {
            var query = _kscHrContext.WorkGroups.AsQueryable()
                .Include(x => x.WorkTime).ThenInclude(x => x.WorkTimeShiftConcepts)
                .ThenInclude(x => x.ShiftConcept).ThenInclude(x => x.ShiftConceptDetails);
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<WorkGroup> GetWorkGroupsByRelations(List<int> id)
        {
            var query = _kscHrContext.WorkGroups.AsQueryable()
                .Include(x => x.WorkTime).ThenInclude(x => x.WorkTimeShiftConcepts)
                .ThenInclude(x => x.ShiftConcept).ThenInclude(x => x.ShiftConceptDetails);
            return query.Where(x => id.Contains(x.Id)).AsQueryable();
        }
        public IQueryable<WorkGroup> GetWorkGroupRelatiedWorkTime()
        {
            var query = _kscHrContext.WorkGroups.AsQueryable().Include(x => x.WorkTime);
            return query;
        }
        public async Task<WorkGroup> GetWorkGroupsByWorkTimeRelations(int id)
        {
            var query = _kscHrContext.WorkGroups.AsQueryable().Include(x => x.WorkTime).AsNoTracking();
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public WorkGroup GetWorkGroupsByWorkTimeRelations_N(int id)
        {
            var query = _kscHrContext.WorkGroups.AsQueryable().Include(x => x.WorkTime).AsNoTracking();
            return query.FirstOrDefault(x => x.Id == id);
        }
        public async Task<WorkGroup> GetWorkGroupIncludWorkTime(int id)
        {
            var query = _kscHrContext.WorkGroups.Where(x => x.Id == id).Include(x => x.WorkTime);
            return await query.FirstOrDefaultAsync();
        }
        public async Task<WorkGroup> GetWorkGroupOutInclud(int id)
        {
            var query = _kscHrContext.WorkGroups.Where(x => x.Id == id).AsNoTracking();
            return await query.FirstOrDefaultAsync();
        }

        public WorkGroup GetWorkGroupsByWorkTimeIdAndCode(int workTimeId, string code)
        {
            var query = _kscHrContext.WorkGroups.AsQueryable().Where(x => x.WorkTimeId == workTimeId
            && x.Code == code).AsNoTracking();
            return query.FirstOrDefault();
        }

        public IQueryable<WorkGroup> GetDataFromWorkGroupForKSCContract()
        {
            return _kscHrContext.WorkGroups
                .IgnoreAutoIncludes()
                .Include(x => x.WorkTime)
                ;
        }
    }
}
