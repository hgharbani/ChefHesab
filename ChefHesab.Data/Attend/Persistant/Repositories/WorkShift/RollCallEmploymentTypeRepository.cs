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
    public class RollCallEmploymentTypeRepository : EfRepository<RollCallEmploymentType, int>, IRollCallEmploymentTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RollCallEmploymentTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<RollCallEmploymentType> GetRollCallEmploymentTypeAsNoTracking()
        {
            return _kscHrContext.RollCallEmploymentTypes.AsNoTracking();
        }
    }
}
