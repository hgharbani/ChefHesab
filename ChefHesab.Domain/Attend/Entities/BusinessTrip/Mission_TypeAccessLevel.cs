using Ksc.HR.Domain.Entities.Workshift;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.BusinessTrip
{
    // TypeAccessLevel
    public class Mission_TypeAccessLevel : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)
        public int? MissionTypeId { get; set; } // MissionTypeId
        public int? AccessLevelId { get; set; } // AccessLevelId
        /// <summary>
        /// ثبت مستقیم ماموریت
        /// </summary>
        public bool ValidMissionCreateDirectly { get; set; } // ValidMissionCreateDirectly
        /// <summary>
        /// ثبت مستقیم بدون گزارش
        /// </summary>
        public bool ValidHasNoMissionReport { get; set; } // ValidHasNoMissionReport


        // Foreign keys

        /// <summary>
        /// Parent AccessLevel pointed by [TypeAccessLevel].([AccessLevelId]) (FK_TypeAccessLevel_AccessLevel)
        /// </summary>
        public virtual AccessLevel AccessLevel { get; set; } // FK_TypeAccessLevel_AccessLevel

        /// <summary>
        /// Parent Mission_Type pointed by [TypeAccessLevel].([MissionTypeId]) (FK_TypeAccessLevel_Type)
        /// </summary>
        public virtual Mission_Type Mission_Type { get; set; } // FK_TypeAccessLevel_Type
    }
}
