using KSC.Domain;

namespace Ksc.HR.Domain.Entities.Chart
{
    public class Chart_JobPositionNature : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? InsertDate { get; set; } // InsertDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public bool IsActive { get; set; } // IsActive

        public virtual ICollection<Chart_JobPosition> Chart_JobPositions { get; set; }
        public virtual ICollection<Chart_JobPositionHistory> JobPositionHistory { get; set; }

        /// <summary>
        /// Child Chart_JobPositionNatureSubGroups where [JobPositionNatureSubGroup].[JobPositionNatureId] point to this entity (FK_JobPositionNatureSubGroup_JobPositionNature)
        /// </summary>
        public virtual ICollection<Chart_JobPositionNatureSubGroup> Chart_JobPositionNatureSubGroups { get; set; } // JobPositionNatureSubGroup.FK_JobPositionNatureSubGroup_JobPositionNature

        public Chart_JobPositionNature()
        {
            JobPositionHistory=new List<Chart_JobPositionHistory>();
            Chart_JobPositions = new List<Chart_JobPosition>();
            Chart_JobPositionNatureSubGroups = new List<Chart_JobPositionNatureSubGroup>();
        }
    }
}
