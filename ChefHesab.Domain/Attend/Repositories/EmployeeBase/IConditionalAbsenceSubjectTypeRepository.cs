using KSC.Domain;
using Ksc.HR.Domain.Entities.Emp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.EmployeeBase;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IConditionalAbsenceSubjectTypeRepository : IRepository<ConditionalAbsenceSubjectType, int>
    {
        IQueryable<ConditionalAbsenceSubjectType> GetRollCallDailyAbsence();
        IQueryable<ConditionalAbsenceSubjectType> GetAllIncludedConditionalAbsenceSubject();
        IQueryable<ConditionalAbsenceSubjectType> GetAllIncludedConditionalAbsenceType();
        ConditionalAbsenceSubjectType GetByConditionalAbsenceSubject_Type(int conditionalAbsenceSubjectId, int conditionalAbsenceTypeId);
    }
}
