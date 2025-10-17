using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Domain.Entities.Chart;


namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public partial class ProvinceRepository : EfRepository<Province,int>, IProvinceRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ProvinceRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<Province> GetAllData()
        {
            return _kscHrContext.Provinces.Include(x=>x.Country).AsQueryable();
        }

    }
}
