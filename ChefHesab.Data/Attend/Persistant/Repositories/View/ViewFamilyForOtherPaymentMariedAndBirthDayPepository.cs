using Ksc.HR.Data.Persistant.Context;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Repositories.View;
using KSC.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.View
{
    public class ViewFamilyForOtherPaymentMariedAndBirthDayPepository : EfRepository<ViewFamilyForOtherPaymentMariedAndBirthDay>, IViewFamilyForOtherPaymentMariedAndBirthDayRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewFamilyForOtherPaymentMariedAndBirthDayPepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;

        }
    }
}
