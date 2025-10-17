using KSC.Domain;
using Ksc.HR.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Emp;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IConditionalAbsenceSubjectRepository : IRepository<ConditionalAbsenceSubject, int>
    {
    }
}
