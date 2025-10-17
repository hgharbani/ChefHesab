using KSC.Domain;
using Ksc.HR.Domain.Entities.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IChart_JobIdentityRepository : IRepository<Chart_JobIdentity, int>
    {
        IQueryable<Chart_JobIdentity> GetChart_JobIdentityById(int id);
        IQueryable<Chart_JobIdentity> GetChart_JobIdentites();
        IQueryable<Chart_JobIdentity> GetAllIncludeJobCategory();
        IQueryable<Chart_JobIdentity> GetChart_JobIdentityByCode(string code);
    }
}
