using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class EventListener : IEntityBase<int>
    {
        public int Id { get; set; }
        public int? StepId { get; set; }
        public int? YearMonth { get; set; }
        public string? Title { get; set; }
        public bool? IsFinished { get; set; }
        public bool? IsError { get; set; }
        public string? ErrorMessage { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
    }
}

