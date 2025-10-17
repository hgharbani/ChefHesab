using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class ConfirmInterdictMessage : IEntityBase<int>
    {
        public int Id { get; set; }
        public int ConfirmInterdictId { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public DateTime SendTime { get; set; }
        public string ConfirmCode { get; set; }
        public virtual ConfirmInterdict ConfirmInterdict { get; set; }
        
    }
}

