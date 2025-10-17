using EFCore.BulkExtensions;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.HR.Share.Model.OtherPaymentStatus;
using KSC.Common.Filters.Models;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class OtherPaymentHeaderRepository : EfRepository<OtherPaymentHeader, int>, IOtherPaymentHeaderRepository
    {
        private readonly KscHrContext _kscHrContext;
        public OtherPaymentHeaderRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderByPaymentDate(int paymentYearMonth)
        {
            var query = _kscHrContext.OtherPaymentHeader.Where(x => x.PaymentYearMonth == paymentYearMonth).Include(x => x.OtherPaymentStatus)
                .Include(x => x.EmployeeOtherPayment)
                 .Include(x => x.AccountBankType)
                 .Include(x => x.AccountCode)
                 ;
            return query;
        }
        public IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderForAccountingSystemByStausId(int stausId)
        {
            var query = _kscHrContext.OtherPaymentHeader.Where(x => x.OtherPaymentStatusId == stausId)
                .Include(x => x.EmployeeOtherPayment).Include(x => x.AccountCode);
            return query;
        }
        public OtherPaymentHeader GetOtherPaymentHeaderByPaymentDateAccountCode(int paymentYearMonth, int accountCodeId)
        {
            var query = _kscHrContext.OtherPaymentHeader.FirstOrDefault(x => x.PaymentYearMonth == paymentYearMonth && x.AccountCodeId == accountCodeId);
            ;
            return query;
        }

        public OtherPaymentHeader GetOtherPaymentHeaderByPaymentDateAccountCodeTrip(int paymentYearMonth, int accountCodeId, int otherPaymentTypeId)
        {
            if (otherPaymentTypeId == 5)
            {
                paymentYearMonth = Convert.ToInt32(paymentYearMonth.ToString().Substring(0, 4));

                var query = _kscHrContext.OtherPaymentHeader.Include(x=>x.AccountCode).Include(x => x.OtherPaymentHeaderTypes).ThenInclude(x => x.OtherPaymentType)
                            .Where(x => Convert.ToInt32(x.PaymentYearMonth.ToString().Substring(0, 4)) == paymentYearMonth && x.AccountCodeId == accountCodeId && x.OtherPaymentHeaderTypes.Any(y => y.OtherPaymentTypeId == otherPaymentTypeId && y.OtherPaymentHeaderId == x.Id))
    .FirstOrDefault();
                return query;

            }
            else
            {
                var query = _kscHrContext.OtherPaymentHeader.Include(x => x.AccountCode).Include(x => x.OtherPaymentHeaderTypes).ThenInclude(x => x.OtherPaymentType)
    .Where(x => x.PaymentYearMonth == paymentYearMonth && x.AccountCodeId == accountCodeId && x.OtherPaymentHeaderTypes.Any(y => y.OtherPaymentTypeId == otherPaymentTypeId && y.OtherPaymentHeaderId == x.Id))
    .FirstOrDefault();

                return query;
            }

        }
        public async Task<bool> AddBulkAsync(OtherPaymentHeader entity)
        {
            try
            {
                List<OtherPaymentHeader> list = new List<OtherPaymentHeader>();
                list.Add(entity);
                await _kscHrContext.BulkInsertOrUpdateAsync(list, option =>
                {
                    option.IncludeGraph = true;
                });

                await _kscHrContext.BulkSaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        public bool IsValidModifiedOtherPaymentHeader(int headerId)
        {
            var otherPaymentHeader = _kscHrContext.OtherPaymentHeader.Find(headerId);

            var validModified = !otherPaymentHeader.AccountingDocumentNumber.HasValue && otherPaymentHeader.OtherPaymentStatusId != EnumOtherPaymentStatus.ConfirmAndSendToAccounting.Id;
            return validModified;
        }
        public IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderByPaymentDateForBlacklist(int paymentYearMonth)
        {
            var query = _kscHrContext.OtherPaymentHeader.Where(x => x.PaymentYearMonth == paymentYearMonth && x.AccountingDocumentNumber == null
            && x.OtherPaymentStatusId != EnumOtherPaymentStatus.ConfirmAndSendToAccounting.Id
            && x.OtherPaymentStatusId != EnumOtherPaymentStatus.SaveDocumentAccounting.Id
            ).Include(x => x.EmployeeOtherPayment);
            return query;
        }
        public IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderByAccountingDocument(int accountingDocumentNumber, DateTime accountingDocumentDate)
        {
            var query = _kscHrContext.OtherPaymentHeader.Where(x => x.AccountingDocumentNumber == accountingDocumentNumber
            && x.AccountingDocumentDate.Value.Date == accountingDocumentDate.Date)
                                ;
            return query;
        }
        public IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderByPaymentYearMonth(int startDate, int endDate, int accountCodeId, int paymentYearMonth, int otherPaymentTypeId)
        {
            var query = _kscHrContext.OtherPaymentHeader
                .Include(x => x.OtherPaymentHeaderTypes)
                .Where(x => x.YearMonthStartReport >= startDate && x.YearMonthEndReport <= endDate
                        && x.AccountCodeId == accountCodeId && x.PaymentYearMonth == paymentYearMonth
                        && x.OtherPaymentHeaderTypes.Any(y => y.OtherPaymentTypeId == otherPaymentTypeId));
            return query;
        }
        public IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderByPaymentYearMonth_Status(int accountCodeId, int paymentYearMonth, int otherPaymentStatusId)
        {
            var query = _kscHrContext.OtherPaymentHeader
                .Include(x => x.EmployeeOtherPayment)
                .Where(x =>
                         x.AccountCodeId == accountCodeId && x.PaymentYearMonth == paymentYearMonth && x.OtherPaymentStatusId == otherPaymentStatusId);
            return query;
        }
        public IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderByPaymentYear(string startYear, int accountCodeId, int paymentYearMonth, int otherPaymentTypeId)
        {
            var query = _kscHrContext.OtherPaymentHeader
                .Include(x => x.OtherPaymentHeaderTypes)
                .Where(x => x.YearMonthStartReport != null && x.YearMonthStartReport.Value.ToString().Substring(0, 4) == startYear
                        && x.AccountCodeId == accountCodeId && x.PaymentYearMonth == paymentYearMonth
                        && x.OtherPaymentHeaderTypes.Any(y => y.OtherPaymentTypeId == otherPaymentTypeId));
            return query;
        }
        public IQueryable<OtherPaymentHeader> GetOtherPaymentHeaderIncludeOtherPaymentHeaderTypes()
        {
            var query = _kscHrContext.OtherPaymentHeader
                .Include(x => x.OtherPaymentHeaderTypes);
            return query;
        }
    }
}
