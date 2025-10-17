using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class CofficientProximityProduct : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)
        public int? Score { get; set; } // Score
        public bool Active { get; set; } // Active

        public ICollection<Chart_JobPosition> Chart_JobPositions { get; set; }
        


        public CofficientProximityProduct()
        {
            Chart_JobPositions = new List<Chart_JobPosition>();
            
        }
    }
}

