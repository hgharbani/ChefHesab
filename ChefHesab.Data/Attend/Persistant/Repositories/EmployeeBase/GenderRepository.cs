using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class GenderRepository : EfRepository<GenderType, int>, IGenderRepository
    {
        private readonly KscHrContext _kscHrContext;
        public GenderRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<GenderType> GetGenderById(int id)
        {
            return _kscHrContext.GenderType.Where(a => a.Id == id);
        }
        public IQueryable<GenderType> GetGenders()
        {
            var result = _kscHrContext.GenderType.AsQueryable();
            return result;
        }
    }
}
