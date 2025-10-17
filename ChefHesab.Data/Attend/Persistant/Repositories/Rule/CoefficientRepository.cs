using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
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
    public class CoefficientRepository : EfRepository<Coefficient, int>, ICoefficientRepository
    {
        private readonly KscHrContext _kscHrContext;

        public CoefficientRepository(KscHrContext context) : base(context)
        {
            _kscHrContext = context;
        }

        public IQueryable<Coefficient> GetCoefficientById(int id)
        {
            return _kscHrContext.Coefficient.Include(x=>x.CoefficientSettings).Where(a => a.Id == id).AsNoTracking();
        }

    }
}
