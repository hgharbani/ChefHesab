using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class ConditionalAbsenceTypeRepository : EfRepository<ConditionalAbsenceType, int>, IConditionalAbsenceTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ConditionalAbsenceTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}
