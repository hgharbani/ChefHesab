using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Repositories.EmployeeBase;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class DependenceJobRepository : EfRepository<DependenceJob, int>, IDependenceJobRepository
    {
        private readonly KscHrContext _kscHrContext;
        public DependenceJobRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}
