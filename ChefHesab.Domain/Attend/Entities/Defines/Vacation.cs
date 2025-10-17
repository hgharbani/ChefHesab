using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class Vacation : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public byte OrderNo { get; set; }

        public bool ShowInHistory { get; set; }

        public bool ShowInManagement { get; set; }

        public bool ReadonlyInManagement { get; set; }
        public virtual ICollection<EmployeeVacationManagement> EmployeeVacationManagement { get; set; } // EmployeeVacationHistory.FK_EmployeeVacationHistory_Employee
public Vacation() {
            EmployeeVacationManagement=new List<EmployeeVacationManagement>();
        }

    }
}

