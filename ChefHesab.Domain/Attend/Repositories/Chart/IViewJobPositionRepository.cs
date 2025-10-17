using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IViewJobPositionRepository : IRepository<ViewJobPosition>
    {
        IQueryable<ViewJobPosition> GetChartJobPositionsAsnotracking();

        //public IEnumerable<ViewMisCostCenter> GetAll1();
        IQueryable<ViewJobPosition> GetChart_JobPositionsIncluded();
        IQueryable<ViewJobPosition> GetChart_JobPositionsIncludedHaveCount();
    }
}

