using KSC.Domain;
using Ksc.HR.Domain.Entities.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Pay;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IOtherPaymentStatusRepository : IRepository<OtherPaymentStatus, int>
    {
    }
}
