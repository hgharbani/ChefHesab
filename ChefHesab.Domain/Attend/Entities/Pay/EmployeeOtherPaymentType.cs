using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.HR.Domain.Entities.Pay
{
    public class EmployeeOtherPaymentType : IEntityBase<long>
    {
        public long Id { get; set; }
        public long EmployeeOtherPaymentId { get; set; }
        public int OtherPaymentTypeId { get; set; }
        public long PaymnetAmountUnit { get; set; }
        public DateTime? PaymentTypeDate { get; set; }
        public int? Year { get; set; }
        public virtual EmployeeOtherPayment EmployeeOtherPayment { get; set; }
        public virtual OtherPaymentType OtherPaymentType { get; set; }
    }
}

