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
    public class WorkTimeShiftConceptRepository : EfRepository<WorkTimeShiftConcept, int>, IWorkTimeShiftConceptRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WorkTimeShiftConceptRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<WorkTimeShiftConcept> GetAllByIncluded()
        {
            return _kscHrContext.WorkTimeShiftConcepts.Include(x => x.WorkTime).Include(x => x.ShiftConcept);
        }
    }
}
