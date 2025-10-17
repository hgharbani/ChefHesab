using Ksc.HR.Domain.Entities.Salary;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Salary
{
    public interface IPaymentTypeRepository : IRepository<PaymentType, int>
    {
    }
}
