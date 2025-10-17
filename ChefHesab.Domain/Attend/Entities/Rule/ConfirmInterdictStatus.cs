using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class ConfirmInterdictStatus : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<ConfirmInterdict> ConfirmInterdict { get; set; }
    }
}

