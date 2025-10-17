using Ksc.HR.Domain.Entities.Rule;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class ConfirmInterdict : IEntityBase<int>
    {
        public int Id { get; set; }
        public int EmployeeInterdictId { get; set; }
        public int ConfirmInterdictStatusId { get; set; }
        public bool IsConfirm { get; set; }
        public DateTime? ConfrimDate { get; set; }
        public bool IsPrint { get; set; }
        public DateTime? PrintDate { get; set; }
        public DateTime InsertDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool IsFinal { get; set; }
        public string Address { get; set; }

        
        public virtual EmployeeInterdict EmployeeInterdict { get; set; }
        public virtual ConfirmInterdictStatus ConfirmInterdictStatus { get; set; }
        public virtual ICollection<ConfirmInterdictMessage> ConfirmInterdictMessage { get; set; }

    }
}

