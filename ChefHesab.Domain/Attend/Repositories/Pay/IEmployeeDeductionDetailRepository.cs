using Ksc.HR.Domain.Entities.Pay;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IEmployeeDeductionDetailRepository : IRepository<EmployeeDeductionDetail, long>
    {
        /// <summary>
        /// به ازای سال ماه مشخص داده هایی که در حالت ایجاد بدهی هستند پیدا میکند
        /// </summary>
        IQueryable<EmployeeDeductionDetail> GetGridConfirmDeduction(int yearMonth, List<int> types);
        IQueryable<EmployeeDeductionDetail> GetEmployeeDeductionDetailByHeaderId(long headerId);
        Task<EmployeeDeductionDetail> GetOne(long id);
        IQueryable<EmployeeDeductionDetail> GetAllByRelatedGrid();
        Task<bool> UpdateBulkAsync(List<EmployeeDeductionDetail> entity);
        void UpdateRange(List<EmployeeDeductionDetail> list);
    }
}
