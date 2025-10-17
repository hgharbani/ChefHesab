using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.HR.Domain.Entities.Pay
{
    public class OtherPaymentStatusFlow : IEntityBase<int>
    {
        public int Id { get; set; }
        public int CurrentStatusId { get; set; }
        public int NextStatusId { get; set; }
        public string UrlName { get; set; }
        public bool IsActive { get; set; }
        public virtual OtherPaymentStatus CurrentStatus { get; set; }
        public virtual OtherPaymentStatus NextStatus { get; set; }
    }
}

