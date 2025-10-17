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
    public class Chart_JobCertificateRepository : EfRepository<Chart_JobCertificate, int>, IChart_JobCertificateRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Chart_JobCertificateRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        

        public IQueryable<Chart_JobCertificate> GetAllData()
        {
            return _kscHrContext.Chart_JobCertificate.AsQueryable();
        }
        public IQueryable<Chart_JobCertificate> GetJobCertificatesByJobPositionId()
        {
            return _kscHrContext.Chart_JobCertificate.Include(a => a.Chart_JobPositionCertificates).AsNoTracking().AsQueryable();
        }
        
    }
}
