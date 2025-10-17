using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories.Rule;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class InterdictDescriptionRepository : EfRepository<InterdictDescription, int>, IInterdictDescriptionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public InterdictDescriptionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<InterdictDescription> GetAllByRelated()
        {
            var result = _kscHrContext.InterdictDescription.Include(x => x.InterdictType).AsQueryable();
            return result;
        }

        public IQueryable<InterdictDescription> GetAllActive()
        {
            var result = _kscHrContext.InterdictDescription.AsQueryable();

            result = result.Where(x => x.IsActive);
            return result;
        }
    }
}
