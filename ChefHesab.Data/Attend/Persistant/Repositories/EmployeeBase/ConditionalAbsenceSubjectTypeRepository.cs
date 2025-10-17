using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class ConditionalAbsenceSubjectTypeRepository : EfRepository<ConditionalAbsenceSubjectType, int>, IConditionalAbsenceSubjectTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ConditionalAbsenceSubjectTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<ConditionalAbsenceSubjectType> GetRollCallDailyAbsence()
        {
            var query = _kscHrContext.ConditionalAbsenceSubjectTypes.Include(x => x.ConditionalAbsenceType)
                .Where(x => x.IsActive && x.ConditionalAbsenceType.IsHourlyAbsence == false);
            return query;
        }
        public IQueryable<ConditionalAbsenceSubjectType> GetAllIncludedConditionalAbsenceType()
        {
            return _kscHrContext.ConditionalAbsenceSubjectTypes.Include(x => x.ConditionalAbsenceType);
        } 
        public IQueryable<ConditionalAbsenceSubjectType> GetAllIncludedConditionalAbsenceSubject()
        {
            return _kscHrContext.ConditionalAbsenceSubjectTypes.Include(x => x.ConditionalAbsenceSubject);
        }
        public ConditionalAbsenceSubjectType GetByConditionalAbsenceSubject_Type(int conditionalAbsenceSubjectId, int conditionalAbsenceTypeId)
        {
            return GetAllIncludedConditionalAbsenceSubject().FirstOrDefault(x => x.ConditionalAbsenceSubjectId == conditionalAbsenceSubjectId && x.ConditionalAbsenceTypeId == conditionalAbsenceTypeId);
        }

    }
}
