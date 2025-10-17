using KSC.Domain;
using Ksc.HR.Domain.Entities.Emp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Pay;

namespace Ksc.HR.Domain.Repositories.Emp
{
    public interface IEmployeeOtherPaymentTypeRepository : IRepository<EmployeeOtherPaymentType, long>
    {
        IQueryable<EmployeeOtherPaymentType> GetAllByYear(int? year);
    }
}
