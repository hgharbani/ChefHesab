using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class StudentRewardRequestRepository : EfRepository<StudentRewardRequest, int>, IStudentRewardRequestRepository
    {
        private readonly KscHrContext _kscHrContext;

        public StudentRewardRequestRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;

        }
        public IQueryable<StudentRewardRequest> GetNoConfirmQueryable()
        {
            var result = _kscHrContext.StudentRewardRequests.AsQueryable().Include(x => x.StudentRewardSetting).ThenInclude(x => x.RewardType).Include(x => x.Employee).Include(x => x.Family).Where(x => x.IsConfirm == false);
            return result;
        }
        public IQueryable<StudentRewardRequest> GetStudentRewardRequestIncludedForReport()
        {
            var result = _kscHrContext.StudentRewardRequests.Include(x => x.StudentRewardSetting)
                .ThenInclude(x => x.RewardType).Include(x => x.Employee).Include(x => x.Family).ThenInclude(x => x.DependenceType).Include(x => x.StudentRewardSetting).ThenInclude(x => x.RewardLevel);
            return result.AsNoTracking();
        }
    }
}
