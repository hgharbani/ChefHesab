using KSC.Domain;
using Ksc.HR.Domain.Entities.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IStudentRewardRequestRepository : IRepository<StudentRewardRequest, int>
    {
        IQueryable<StudentRewardRequest> GetNoConfirmQueryable();
        IQueryable<StudentRewardRequest> GetStudentRewardRequestIncludedForReport();
    }
}
