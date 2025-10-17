using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;
namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class PaymentAdditionalSettingJobPositionRepository : EfRepository<PaymentAdditionalSettingJobPosition, int>, IPaymentAdditionalSettingJobPositionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public PaymentAdditionalSettingJobPositionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<PaymentAdditionalSettingJobPosition> GetAllQueryableBySettingId()
        {
            return _kscHrContext.PaymentAdditionalSettingJobPosition.AsQueryable().Include(x => x.JobPosition)
                .ThenInclude(x=>x.EmployeeJobPositions).ThenInclude(x=>x.Employee);
        }
    }
}

