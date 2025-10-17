using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Repositories.Chart;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class BloodTypeRepository : EfRepository<BloodType, int>, IBloodTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public BloodTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<BloodType> GetBloodTypeById(int id)
        {
            return _kscHrContext.BloodType.Where(a => a.Id == id);
        }
        public IQueryable<BloodType> GetBloodTypes()
        {
            var result = _kscHrContext.BloodType.AsQueryable();
            return result;
        }
    }
}
