using KSC.Domain;
using Ksc.HR.Domain.Entities.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IOtherPaymentHeaderRepository : IRepository<OtherPaymentHeader, int>
    {
        Task<bool> AddBulkAsync(OtherPaymentHeader entity);
        IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderByAccountingDocument(int accountingDocumentNumber, DateTime accountingDocumentDate);
        IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderByPaymentDate(int paymentYearMonth);
        OtherPaymentHeader GetOtherPaymentHeaderByPaymentDateAccountCode(int paymentYearMonth, int accountCodeId);
        OtherPaymentHeader GetOtherPaymentHeaderByPaymentDateAccountCodeTrip(int paymentYearMonth, int accountCodeId, int otherPaymentTypeId);

        IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderByPaymentDateForBlacklist(int paymentYearMonth);
        IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderByPaymentYear(string startYear, int accountCodeId, int paymentYearMonth, int otherPaymentTypeId);
        IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderByPaymentYearMonth(int startDate, int endDate, int accountCodeId, int paymentYearMonth, int typeId);
        IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderByPaymentYearMonth_Status(int accountCodeId, int paymentYearMonth, int otherPaymentStatusId);
        IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderForAccountingSystemByStausId(int stausId);
        IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderIncludeOtherPaymentHeaderTypes();
        bool IsValidModifiedOtherPaymentHeader(int headerId);
    }
}
