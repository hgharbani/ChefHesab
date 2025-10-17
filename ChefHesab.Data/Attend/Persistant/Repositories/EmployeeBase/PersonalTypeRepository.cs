using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class PersonalTypeRepository : EfRepository<PersonalType, int>, IPersonalTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public PersonalTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<PersonalType> GetPersonalTypeById(int id)
        {
            return _kscHrContext.PersonalType.Where(a => a.Id == id);
        }
        public IQueryable<PersonalType> GetPersonalTypes()
        {
            var result = _kscHrContext.PersonalType.AsQueryable();
            return result;
        }
    }
}
