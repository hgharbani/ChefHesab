using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class HrOption : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? OptionTypeId { get; set; }
        public string ValueString { get; set; }
        public DateTime? ValueTime { get; set; }
        public double? ValueFloat { get; set; }
        public System.Byte? Code { get; set; }
    }
}

