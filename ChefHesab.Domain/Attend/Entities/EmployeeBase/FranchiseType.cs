using Ksc.HR.Domain.Entities.Emp;
using KSC.Domain;

namespace Ksc.HR.Domain.Entities.EmployeeBase
{
    public class FranchiseType : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)
        public string Title { get; set; } // Title
        public float DefualtValue { get; set; }
        public virtual ICollection<InsuranceBooklet> InsuranceBooklets { get; set; } // InsuranceBooklet.FK_InsuranceBooklet_FranchiseType

        public FranchiseType()
        {
            InsuranceBooklets = new List<InsuranceBooklet>();
        }
    }
}
