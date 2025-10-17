using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Entities.Salary;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.HR.Domain.Entities.Pay
{
    public class OtherPaymentHeader : IEntityBase<int>
    {
        public int Id { get; set; }
        public int PaymentYearMonth { get; set; }
        public int AccountCodeId { get; set; }
        public int OtherPaymentStatusId { get; set; }
        public int? AccountingDocumentNumber { get; set; }
        public DateTime? AccountingDocumentDate { get; set; }
        public int? BankId { get; set; }
        public int? AccountBankTypeId { get; set; }
        public bool ShowInPortal { get; set; }
        public int? YearMonthStartReport { get; set; }
        public int? YearMonthEndReport { get; set; }
        /// <summary>
        /// توضیحات برای نمایش در پورتال
        /// </summary>
        public string DescriptionForPortal { get; set; } // DescriptionForPortal

        /// <summary>
        /// تاریخ پرداخت برای نمایش در پورتال
        /// </summary>
        public DateTime? PaymentDateForPortal { get; set; } // PaymentDateForPortal
        public virtual AccountCode AccountCode { get; set; }
        public virtual OtherPaymentStatus OtherPaymentStatus { get; set; }
        public virtual AccountBankType AccountBankType { get; set; }
        public virtual ICollection<EmployeeOtherPayment> EmployeeOtherPayment { get; set; }
        public virtual ICollection<OtherPaymentDetail> OtherPaymentDetail { get; set; }
        public virtual ICollection<OtherPaymentHeaderType> OtherPaymentHeaderTypes { get; set; } // OtherPaymentHeaderType.FK_OtherPaymentHeaderType_OtherPaymentHeader
 
        public OtherPaymentHeader()
        {
            EmployeeOtherPayment = new List<EmployeeOtherPayment>();
            OtherPaymentDetail = new List<OtherPaymentDetail>();
            OtherPaymentHeaderTypes = new List<OtherPaymentHeaderType>();
        }


    }
}

