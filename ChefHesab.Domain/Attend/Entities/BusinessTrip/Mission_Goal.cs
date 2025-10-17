using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.BusinessTrip
{
    //Goal
    /// <summary>
    /// هدف ماموریت
    /// </summary>
    public class Mission_Goal : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// هدف ماموریت
        /// </summary>
        public string Title { get; set; } // Title (length: 50)

        // Reverse navigation

        /// <summary>
        /// Child Mission_Requests where [Request].[MissionGoalId] point to this entity (FK_Request_Goal)
        /// </summary>
        public bool IsActive { get; set; } // IsActive
        public virtual ICollection<Mission_Request> Mission_Requests { get; set; } // Request.FK_Request_Goal
        public string UpdateUser { get; set; }
        public string InsertUser { get; set; }
        public string DomainName { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? InsertDate { get; set; }
        public byte[] RowVersion { get; set; }

        public Mission_Goal()
        {
            Mission_Requests = new List<Mission_Request>();
        }
    }
}
