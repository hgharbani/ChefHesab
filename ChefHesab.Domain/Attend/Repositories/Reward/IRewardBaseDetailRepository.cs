using KSC.Domain;
using Ksc.HR.Domain.Entities.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Entities.Reward;

namespace Ksc.HR.Domain.Repositories.Reward
{
    public interface IRewardBaseDetailRepository : IRepository<RewardBaseDetail, int>
    {
        IQueryable<RewardBaseDetail> GetAllActive();
        IQueryable<RewardBaseDetail> GetByBaseHeaderId(int BaseHeaderId, int? UnitTypeId);
        double GetkaraneMabnaMah(int headerId, double ToolidMoadel);
    }
}
