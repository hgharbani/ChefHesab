using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Reward;
using KSC.Domain;
namespace Ksc.Hr.Domain.Entities
{
    public class ProductionEfficiency : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Code { get; set; }
        public bool? Active { get; set; }
        /// <summary>
        /// ضریب پاداش
        /// </summary>
        public Double? CPercent { get; set; }

        
        public ICollection<Chart_JobPosition> Chart_JobPositions { get; set; }
        /// <summary>
        /// Child RewardInSmcMonthlyProductions where [RewardInSmcMonthlyProduction].[RewardInId] point to this entity (FK_RewardInMonthlyProduction_RewardIn)
        /// </summary>
        public virtual ICollection<RewardInSmcMonthlyProduction> RewardInSmcMonthlyProductions { get; set; } // RewardInSmcMonthlyProduction.FK_RewardInMonthlyProduction_RewardIn
        public virtual ICollection<RewardInQualityControlMonthlyPelet> RewardInQualityControlMonthlyPelets { get; set; } 
        public virtual ICollection<RewardInQualityControlMonthlyDri> RewardInQualityControlMonthlyDris { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

        public ProductionEfficiency()
        {
            Chart_JobPositions = new List<Chart_JobPosition>();
            RewardInSmcMonthlyProductions = new List<RewardInSmcMonthlyProduction>();
            RewardInQualityControlMonthlyPelets = new List<RewardInQualityControlMonthlyPelet>();
            RewardInQualityControlMonthlyDris = new List<RewardInQualityControlMonthlyDri>();
            Employees = new List<Employee>();
        }
    }
}

