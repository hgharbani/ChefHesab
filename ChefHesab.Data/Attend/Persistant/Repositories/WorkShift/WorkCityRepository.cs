using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class WorkCityRepository : EfRepository<WorkCity, int>, IWorkCityRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WorkCityRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<WorkCity> GetCityWithProvice()
        {
            var query = _kscHrContext.WorkCities.AsQueryable().Include(x => x.City).ThenInclude(x => x.Province).Include(x=>x.Company).AsQueryable();
            return query;

        }

    }
}
