using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class EducationRepository : EfRepository<Education, int>, IEducationRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EducationRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<Education> GetEducationById(int id)
        {
            return  _kscHrContext.Education.Where(a => a.Id == id);
        }

        public IQueryable<Education> GetEducationByIds(List<int> ids)
        {
            return _kscHrContext.Education.Where(a => ids.Contains(a.Id));
        }
        
        public IQueryable<Education> GetEducations()
        {
            var result = _kscHrContext.Education.AsQueryable();
            return result;
        }
    }
}
