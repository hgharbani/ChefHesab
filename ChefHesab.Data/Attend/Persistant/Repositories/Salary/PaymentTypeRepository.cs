using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories.Salary;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Salary
{
    public  class PaymentTypeRepository : EfRepository<PaymentType, int>, IPaymentTypeRepository
    {
        public PaymentTypeRepository(KscHrContext context) : base(context)
        {
        }
    }
}
