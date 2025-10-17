using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.HR.Domain.Entities.Personal
{
    public class EmployeeBreastfedding : IEntityBase<int>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime BreastfeddingStartDate { get; set; }
        public DateTime BreastfeddingEndDate { get; set; }
        public bool IsBreastfeddingInStartShift { get; set; }
        public DateTime InsertDate { get; set; }
        public string InsertUser { get; set; }
       // public DateTime UpdateDate { get; set; }
       // public string UpdateUser { get; set; }
        public bool IsActive { get; set; }
        public virtual Employee Employee { get; set; }
    }
}

