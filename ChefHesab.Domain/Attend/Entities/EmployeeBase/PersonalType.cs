using KSC.Domain;
using Ksc.HR.Domain.Entities.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.EmployeeBase
{
    public class PersonalType : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)
        public string Title { get; set; } // Title
        public string Description { get; set; } // Descrition
        public DateTime? InsertDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool IsActive { get; set; }


        public virtual ICollection<Employee> Employees { get; set; }

        public PersonalType()
        {
            Employees = new List<Employee>();
        }
    }

}
