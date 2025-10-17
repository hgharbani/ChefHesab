using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class IsarStatusRepository : EfRepository<IsarStatus, int>, IIsarStatusRepository
    {
        private readonly KscHrContext _kscHrContext;
        public IsarStatusRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<IsarStatus> GetIsarStatusById(int id)
        {
            return _kscHrContext.IsarStatus.Where(a => a.Id == id);
        }
        public IQueryable<IsarStatus> GetIsarStatus()
        {
            var result = _kscHrContext.IsarStatus.AsQueryable();
            return result;
        }
    }
}
