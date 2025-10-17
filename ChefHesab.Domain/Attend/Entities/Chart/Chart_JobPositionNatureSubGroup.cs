using KSC.Domain;

namespace Ksc.HR.Domain.Entities.Chart
{
    // JobPositionNatureSubGroup
    /// <summary>
    /// رسته
    /// </summary>
    public class Chart_JobPositionNatureSubGroup : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)
        public int JobPositionNatureId { get; set; } // JobPositionNatureId
        public string Title { get; set; } // Title (length: 200)

        public decimal? GPercent { get; set; } // GPercent decimal(18,2)
        public string Code { get; set; } // Code (length: 200)
        public bool IsActive { get; set; } // IsActive
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? InsertDate { get; set; } // InsertDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate

        // Foreign keys

        /// <summary>
        /// Parent Chart_JobPositionNature pointed by [JobPositionNatureSubGroup].([JobPositionNatureId]) (FK_JobPositionNatureSubGroup_JobPositionNature)
        /// </summary>
        public virtual Chart_JobPositionNature Chart_JobPositionNature { get; set; } // FK_JobPositionNatureSubGroup_JobPositionNature
    }
}
