using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.HR.Domain.Entities.Pay
{
    public class EmployeeOtherPaymentHistory : IEntityBase<long>
    {
        public long Id { get; set; }
        public long EmployeeOtherPaymentId { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string RemoteIpAddress { get; set; }
        public string AuthenticateUserName { get; set; }
        public string HistoryDescription { get; set; }
        public virtual EmployeeOtherPayment EmployeeOtherPayment { get; set; }
    }
}

