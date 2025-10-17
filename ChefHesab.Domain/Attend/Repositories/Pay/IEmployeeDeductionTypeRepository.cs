using KSC.Domain;
using Ksc.HR.Domain.Entities.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IEmployeeDeductionTypeRepository : IRepository<EmployeeDeductionType, int>
    {
        /// <summary>
        /// با انتخاب نوع بدهی کد حساب پرداخت و شرکت اعتبار دهنده را پیدا میکند
        /// </summary>
        EmployeeDeductionType GetByIdWithIncludedAccountCode(int id, bool asNoTraking = false);
    }
}
