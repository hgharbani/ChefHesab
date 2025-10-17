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
    public class InvalidDayTypeInForcedOvertimeRepository : EfRepository<InvalidDayTypeInForcedOvertime, int>, IInvalidDayTypeInForcedOvertimeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public InvalidDayTypeInForcedOvertimeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public List<int> GetInvalidDayTypeInForcedOvertimeByWorkTime(int workTimeId)
        {
            var query = _kscHrContext.InvalidDayTypeInForcedOvertimes.Where(x => x.IsActive && x.WorkTimeId == workTimeId).Select(x => x.WorkDayTypeId).ToList();
            return query;
        }
    }
}
