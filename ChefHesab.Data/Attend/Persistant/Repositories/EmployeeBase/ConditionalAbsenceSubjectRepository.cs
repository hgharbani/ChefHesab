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
    public class ConditionalAbsenceSubjectRepository : EfRepository<ConditionalAbsenceSubject, int>, IConditionalAbsenceSubjectRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ConditionalAbsenceSubjectRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}
