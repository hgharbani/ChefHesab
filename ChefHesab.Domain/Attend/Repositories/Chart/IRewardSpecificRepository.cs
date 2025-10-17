using KSC.Domain;
using Ksc.HR.Domain.Entities.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IRewardSpecificRepository : IRepository<Chart_RewardSpecific, int>
    {
        IQueryable<Chart_RewardSpecific> GetRewardSpecificById(int id);
        IQueryable<Chart_RewardSpecific> GetRewardSpecifics();
    }
}
