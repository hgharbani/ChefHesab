using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    // LicenseType
    /// <summary>
    /// انواع مجوز
    /// </summary>
    public class LicenseType : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)
        public string Title { get; set; } // Title (length: 250)
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? InsertDate { get; set; } // InsertDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public bool IsActive { get; set; } // IsActive

        // Reverse navigation

        /// <summary>
        /// Child Chart_LicenseJobPositions where [LicenseJobPosition].[LicenseTypeId] point to this entity (FK_LicenseJobPosition_LicenseType)
        /// </summary>
        public virtual ICollection<LicenseJobPosition> LicenseJobPositions { get; set; } // LicenseJobPosition.FK_LicenseJobPosition_LicenseType

        public LicenseType()
        {
            LicenseJobPositions = new List<LicenseJobPosition>();
        }
    }
}

