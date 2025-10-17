using KSC.Domain;
using Ksc.HR.Domain.Entities.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.Pay;
using KSC.Common;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IEmployeeOtherPaymentRepository : IRepository<EmployeeOtherPayment, long>
    {
        IQueryable<OtherPaymentHeader> IsExistEmployeeOtherPaymentInMonthYear(int startDate, int endDate, int otherPaymentType, int salaryDate);

        IQueryable<EmployeeOtherPayment> GetOtherPaymentWithHeaderIdForTextFileBank(int headerId);
        IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByHeaderId(int headerId);
        IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByHeaderIdForDelete(int headerId);
        IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByHeaderIdIncludedEmployee(int headerId);
        IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByHeaderIdNotIncluded(int headerId);
        IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByMonthYear_AccountCode(int startDate, int endDate, int accountCodeId);
        IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentInMonthYear(int startDate, int endDate, int otherPaymentType);
        IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentManagmentInMonthYear(int startDate, int endDate, int otherPaymentType, List<int> ids);
        bool IsExistEmployeeOtherPaymentInMonthYear(int startDate, int endDate, int otherPaymentType);
        //bool GetPermitSettingByFilter(int startDate, int endDate, int otherPaymentType);
        IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByHeaderIncluded();
        IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentForPrivatePortal(int employeeId, int paymnetMonth);
        EmployeeOtherPayment GetEmployeeOtherPaymentIncludeHeaderById(int id);
        IQueryable<EmployeeOtherPayment> GetEmployeeOtherPaymentByHeaderIds(List<int> headerIds);
        Task<KscResult> EmployeeOtherPaymentManagementVacationSold(OtherPaymentModel model);
        Task<IQueryable<EmployeeOtherPayment>> GetEmployeeOtherPaymentReport();
        Task<KscResult> InsertEmployeesOtherPaymentManagement(OtherPaymentModel model);
        Task<KscResult> InsertSingleEmployeeOtherPaymentManagement(OtherPaymentModel model);
    }
}
