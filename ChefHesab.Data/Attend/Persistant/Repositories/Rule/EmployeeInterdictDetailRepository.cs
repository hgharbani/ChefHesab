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
    public class EmployeeInterdictDetailRepository : EfRepository<EmployeeInterdictDetail, int>, IEmployeeInterdictDetailRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeInterdictDetailRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<EmployeeInterdictDetail> GetInterdictDetailByInterdictId(int id)
        {
            return _kscHrContext.EmployeeInterdictDetails
                .Include(x=>x.AccountCode)
                .ThenInclude(x=>x.InterdictCategory)
                .Where(x => x.EmployeeInterdictId == id && x.IsActive);
        }
        public IQueryable<EmployeeInterdictDetail> GetInterdictDetailByInterdictIds(int[] ids)
        {
            return _kscHrContext.EmployeeInterdictDetails
                .Include(x=>x.AccountCode)
                .ThenInclude(x=>x.InterdictCategory)
                .Where(x => ids.Contains(x.EmployeeInterdictId) && x.IsActive);
        }
    }
}
