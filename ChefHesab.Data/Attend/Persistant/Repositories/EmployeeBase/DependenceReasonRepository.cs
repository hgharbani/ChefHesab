using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ksc.HR.Domain.Repositories.EmployeeBase;
using Ksc.HR.Domain.Entities;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class DependenceReasonRepository : EfRepository<DependenceReason, int>, IDependenceReasonRepository
    {
        private readonly KscHrContext _kscHrContext;
        public DependenceReasonRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<DependenceReason> GetDataFromDependenceReasonForKSCContract()
        {
            return _kscHrContext.DependenceReason
                .IgnoreAutoIncludes();
        }
    }
}
