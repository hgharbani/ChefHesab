using KSC.Domain;
using Ksc.HR.Domain.Entities.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IChart_JobPositionStatusRepository : IRepository<Chart_JobPositionStatus, int>
    {
        IQueryable<Chart_JobPositionStatus> GetAllIncludeCategory();
        IQueryable<Chart_JobPositionStatus> GetChart_JobPositionStatusById(int id);
        IQueryable<Chart_JobPositionStatus> GetChart_JobPositionStatuses();
    }
}
