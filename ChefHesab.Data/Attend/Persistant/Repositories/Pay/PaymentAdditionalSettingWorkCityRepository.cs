using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class PaymentAdditionalSettingWorkCityRepository : EfRepository<PaymentAdditionalSettingWorkCity, int>, IPaymentAdditionalSettingWorkCityRepository
    {

        private readonly KscHrContext _kscHrContext;
        public PaymentAdditionalSettingWorkCityRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}
