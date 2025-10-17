using KSC.Domain;
using Ksc.HR.Domain.Entities.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IJobPositionHistoryRepository : IRepository<Chart_JobPositionHistory, long>
    {
        IQueryable<Chart_JobPositionHistory> GetJobPositionHistoryById(long id);
        IQueryable<Chart_JobPositionHistory> GetJobPositionHistorys();
        IQueryable<Chart_JobPositionHistory> GetJobPositionHistoryByJobIds(int id);
    }
}
