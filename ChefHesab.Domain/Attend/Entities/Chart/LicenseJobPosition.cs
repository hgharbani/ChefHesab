using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    // LicenseJobPosition
    /// <summary>
    /// اختیارات پست
    /// </summary>
    public class LicenseJobPosition : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// نوع اختیارات
        /// </summary>
        public int LicenseTypeId { get; set; } // LicenseTypeId

        /// <summary>
        /// پست
        /// </summary>
        public int JobPositionId { get; set; } // JobPositionId
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? InsertDate { get; set; } // InsertDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public bool IsActive { get; set; } // IsActive
        public string CreateRemoteIpAddress { get; set; } // CreateRemoteIpAddress (length: 50)
        public string UpdateRemoteIpAddress { get; set; } // UpdateRemoteIpAddress (length: 50)
        public string CreateAuthenticateUserName { get; set; } // CreateAuthenticateUserName (length: 50)
        public string UpdateAuthenticateUserName { get; set; } // UpdateAuthenticateUserName (length: 50)

        // Foreign keys

        /// <summary>
        /// Parent JobPosition pointed by [LicenseJobPosition].([JobPositionId]) (FK_LicenseJobPosition_JobPosition)
        /// </summary>
        public virtual Chart_JobPosition JobPosition { get; set; } // FK_LicenseJobPosition_JobPosition

        /// <summary>
        /// Parent LicenseType pointed by [LicenseJobPosition].([LicenseTypeId]) (FK_LicenseJobPosition_LicenseType)
        /// </summary>
        public virtual LicenseType LicenseType { get; set; } // FK_LicenseJobPosition_LicenseType
    }
}

