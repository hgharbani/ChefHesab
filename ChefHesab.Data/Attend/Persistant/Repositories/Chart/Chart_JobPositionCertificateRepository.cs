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
    public class Chart_JobPositionCertificateRepository : EfRepository<Chart_JobPositionCertificate, int>, IChart_JobPositionCertificateRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Chart_JobPositionCertificateRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        

        public IQueryable<Chart_JobPositionCertificate> GetAllData()
        {
            return _kscHrContext.Chart_JobPositionCertificate.AsQueryable();
        }
        //public IQueryable<Chart_JobPositionCertificate> GetChart_JobPositionCertificates()
        //{
        //    var result = _kscHrContext.Chart_JobPositionCertificate.AsQueryable();
        //    return result;
        //}
    }
}
