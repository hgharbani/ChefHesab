using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class EmployeeHaveTravel : IEntityBase<long>
    {
        public long Id { get; set; }
        public int EmployeeId { get; set; }
        public int CheckDate { get; set; }
        public int? YearMonth { get; set; }
    }
}

