using KSC.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Chart;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IChart_JobPositionCertificateRepository : IRepository<Chart_JobPositionCertificate, int>
    {
        //IQueryable<Chart_JobCertificate> GetChart_JobCertificateById(int id);
        //IQueryable<Chart_JobCertificate> GetChart_JobCertificatesAsNoTracking();
        //IQueryable<Chart_JobCertificate> GetChart_JobCertificatesByJobPositionId();
    }
}
