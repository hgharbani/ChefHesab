using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.HR.Domain.Entities.Pay
{
    public class EmployeeOtherPayment : IEntityBase<long>
    {
        //
        public long Id { get; set; }
        public int EmployeeId { get; set; }
        public int OtherPaymentHeaderId { get; set; }
        public string CostCenterCode { get; set; }
        public int? YearMonthStartReport { get; set; }
        public int? YearMonthEndReport { get; set; }
        public long PaymentAmount { get; set; }
        public int? PaymentPersonCount { get; set; }
        public bool IsBlacklist { get; set; }
        public bool InvalidShowInPortal { get; set; }
        public bool InvalidPayment { get; set; }
        public string AccountNumber { get; set; }
        public int? DefaultPaymentPersonCount { get; set; }
        public long DefaultPaymentAmount { get; set; }
        public string InsertUser { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string RemoteIpAddress { get; set; }
        public string AuthenticateUserName { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual OtherPaymentHeader OtherPaymentHeader { get; set; }
        public virtual ICollection<EmployeeOtherPaymentHistory> EmployeeOtherPaymentHistory { get; set; }
        public virtual ICollection<EmployeeOtherPaymentType> EmployeeOtherPaymentType { get; set; }

        public EmployeeOtherPayment()
        {
            EmployeeOtherPaymentHistory = new List<EmployeeOtherPaymentHistory>();
            EmployeeOtherPaymentType=new List<EmployeeOtherPaymentType>();  
        }
    }
}

