using Ksc.HR.Domain.Entities.Pay;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.DeductionAdditional;
using Ksc.HR.Share.Model.Pay;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IEmployeeDeductionHeaderRepository : IRepository<EmployeeDeductionHeader, long>
    {
        IQueryable<EmployeeDeductionHeader> GetEmployeeDeductionHeader();
        IQueryable<EmployeeDeductionHeader> GetEmployeeDeductionHeaderByRelated();
        IQueryable<EmployeeDeductionHeader> GetEmployeeDeductionHeaderByRole(List<string> roles);
        void InsertEmployeeDeduction(EmployeeDeductionDto dto);
        long GetHeaderIdBybasisId(int YearMonth, int EmployeeId, int EmployeeDeductionTypeId);
        long GetHeaderIdByKey(int keyId);
        Task<bool> AddBulkAsync(List<EmployeeDeductionHeader> list);
         bool RemoveEmployeeDeductionById(long id);
        IQueryable<EmployeeDeductionHeader> GetEmployeeDeductionHeaderDetail();

       
        List<PaidableEmployeeDeductionListDto> Sp_PaidableEmployeeDeduction(int startShamsiDate, int accountCodeId, int deductionTypeId);
    }
}
