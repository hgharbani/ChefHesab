using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
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
    public class NationalityRepository : EfRepository<Nationality, int>, INationalityRepository
    {
        private readonly KscHrContext _kscHrContext;
        public NationalityRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<Nationality> GetNationalities()
        {
            var result = _kscHrContext.Nationality.AsQueryable();
            return result;
        }

        public IQueryable<Nationality> GetNationalityById(int id)
        {
            return _kscHrContext.Nationality.Where(a => a.Id == id);
        }

        public IQueryable<Nationality> GetDataFromNationalityForKSCContract()
        {
            return _kscHrContext.Nationality.IgnoreAutoIncludes();
        }
    }
}
