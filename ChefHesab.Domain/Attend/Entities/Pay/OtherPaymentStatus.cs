using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.HR.Domain.Entities.Pay
{
    public class OtherPaymentStatus : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public virtual ICollection<OtherPaymentDetail> OtherPaymentDetail { get; set; }
        public virtual ICollection<OtherPaymentHeader> OtherPaymentHeader { get; set; }
        public virtual ICollection<OtherPaymentStatusFlow> CurrentStatusOtherPaymentStatusFlow { get; set; }
        public virtual ICollection<OtherPaymentStatusFlow> NextStatusOtherPaymentStatusFlow { get; set; }
        /// <summary>
        /// Child Pay_PaymentHeaders where [PaymentHeader].[OtherPaymentStatusId] point to this entity (FK_PaymentHeader_OtherPaymentStatus)
        /// </summary>
        public virtual ICollection<PaymentHeader> PaymentHeaders { get; set; } // PaymentHeader.FK_PaymentHeader_OtherPaymentStatus

        public OtherPaymentStatus()
        {
            CurrentStatusOtherPaymentStatusFlow=new List<OtherPaymentStatusFlow>();
            NextStatusOtherPaymentStatusFlow = new List<OtherPaymentStatusFlow>();
            OtherPaymentDetail = new List<OtherPaymentDetail>();
            OtherPaymentHeader = new List<OtherPaymentHeader>();
            PaymentHeaders = new List<PaymentHeader>();
        }
    }
}

