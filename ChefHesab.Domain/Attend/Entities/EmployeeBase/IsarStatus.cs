using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
namespace Ksc.HR.Domain.Entities.EmployeeBase
{
    public class IsarStatus : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public IsarStatus()
        {
            Employees = new List<Employee>();
        }
    }
}

