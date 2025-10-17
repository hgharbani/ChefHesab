using KSC.Domain;
using Ksc.HR.Domain.Entities.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IJobPositionNatureRepository : IRepository<Chart_JobPositionNature, int>
    {
        IQueryable<Chart_JobPositionNature> GetJobPositionNatureById(int id);
        IQueryable<Chart_JobPositionNature> GetJobPositionNatures();
    }
}
