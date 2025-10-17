using KSC.Domain;
using Ksc.Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.EmployeeBase;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IPersonalTypeRepository : IRepository<PersonalType, int>
    {
        IQueryable<PersonalType> GetPersonalTypeById(int id);
        IQueryable<PersonalType> GetPersonalTypes();
    }
}
