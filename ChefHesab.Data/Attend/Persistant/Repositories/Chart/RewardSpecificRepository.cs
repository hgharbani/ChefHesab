using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public partial class RewardSpecificRepository : EfRepository<Chart_RewardSpecific, int>, IRewardSpecificRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardSpecificRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<Chart_RewardSpecific> GetRewardSpecificById(int id)
        {
            return _kscHrContext.Chart_RewardSpecific.Where(a => a.Id == id);
        }
        public IQueryable<Chart_RewardSpecific> GetRewardSpecifics()
        {
            var result = _kscHrContext.Chart_RewardSpecific.AsQueryable();
            return result;
        }
       
    }
}
